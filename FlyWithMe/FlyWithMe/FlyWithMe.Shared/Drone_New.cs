using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.System.Threading;

namespace FlyWithMe
{
    /// <summary>
    /// Refactored Version of the Drone Objekt
    /// Contains properties and Methods to use more easily
    /// </summary>
    class Drone_New
    {
        public DroneState State { get; set; }
        public DroneConnection Connection { get; }
        public EventHandler<CustomEventArgs> SomethingChanged;
        private short pitch;
        private short roll;
        private short up;
        private short yaw;

        public Drone_New()
        {
            Connection = new DroneConnection();
        }

        private int ticks = 0;
        private async void HandleTick(ThreadPoolTimer timer)
        {
            if (State == DroneState.Flying || State == DroneState.Hovering)
            {
                if (ShouldMove())
                {
                    await Connection.SendMovementCommand(new MoveCommandBytes(pitch, roll, yaw, up, 5));
                }
                else
                {
                    await Connection.SendMovementCommand(Commands.Hover);
                }
            }
            ticks++;
        }

        // Drone seems to require hovering or movement commands every 50ms
        private void StartMoveLoop()
        {
            ThreadPoolTimer.CreatePeriodicTimer(HandleTick, TimeSpan.FromMilliseconds(50));
        }

        private bool ShouldMove()
        {
            if (pitch != 0) return true;
            if (roll != 0) return true;
            if (yaw != 0) return true;
            if (up != 0) return true;
            return false;
        }

        public async Task Connect()
        {
            await Connection.Connect();
        }

        public async Task TakeOff()
        {
            await Connection.SendSimpleCommand(Commands.TakeOff);

            StartMoveLoop();
        }

        public async Task Land()
        {
            await Connection.SendSimpleCommand(Commands.Landing);
        }
        
        public async Task EmergencyStopp()
        {
            await Connection.SendSimpleCommand(Commands.EmergencyShutdown);
        }
        
        public async Task Move(Direction direction, short speed)
        {
            pitch = 0;
            roll = 0;
            yaw = 0;
            up = 0;

            switch (direction)
            {
                case Direction.Front:
                    pitch = speed;
                    break;
                case Direction.Back:
                    pitch = (short) (0 - speed);
                    break;
                case Direction.Right:
                    roll = speed;
                    break;
                case Direction.Left:
                    roll = (short) (0 - speed);
                    break;
                case Direction.Up:
                    up = speed;
                    break;
                case Direction.Down:
                    up = (short)(0 - speed);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }

        public async Task Flip(Direction direction)
        {
            switch (direction)
            {
                case Direction.Front:
                    await Connection.SendSimpleCommand(Commands.ForwardFlip);
                    break;
                case Direction.Back:
                    await Connection.SendSimpleCommand(Commands.BackwardsFlip);
                    break;
                case Direction.Right:
                    await Connection.SendSimpleCommand(Commands.RightFlip);
                    break;
                case Direction.Left:
                    await Connection.SendSimpleCommand(Commands.LeftFlip);
                    break;
                case Direction.Up:
                case Direction.Down:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }

        public async Task Move(short roll, short pitch, short yaw, short up)
        {
            this.roll = roll;
            this.pitch = pitch;
            this.yaw = yaw;
            this.up = up;
        }

        public async Task Rotate(short yaw)
        {
            this.yaw = yaw;
        }

        private void OnHandle_SomethingChanged(object o, GattValueChangedEventArgs args)
        {
            var sender = (GattCharacteristic)o;
            var senderData = sender.Uuid;
            var byteArray = args.CharacteristicValue.ToArray();

            if (senderData == ParrotUuids.Characteristic_B0E_DroneState)
            {
                var incomingDataIsDroneState = (byteArray[0] == 4)
                        && (byteArray[2] == 2)
                        && (byteArray[3] == 3)
                        && (byteArray[4] == 1);

                if (incomingDataIsDroneState)
                {
                    var receivedDroneState = byteArray[6];
                    switch (receivedDroneState)
                    {
                        case 0:
                            State = DroneState.Landed;
                            break;
                        case 1:
                            State = DroneState.TakeingOff;
                            break;
                        case 2:
                            State = DroneState.Hovering;
                            break;
                        case 3:
                            State = DroneState.Flying;
                            break;
                        case 4:
                            State = DroneState.Landing;
                            break;
                        case 5:
                            State = DroneState.Emergency;
                            break;
                        case 6:
                            State = DroneState.Rolling;
                            break;
                    }
                }

                SomethingChanged?.Invoke(this, new CustomEventArgs("Drone State: " + State));
                return;
            }

            if (senderData == ParrotUuids.Characteristic_B0F_Battery)
            {
                SomethingChanged?.Invoke(this, new CustomEventArgs("Battery: " + BitConverter.ToString(byteArray)));
                return;
            }

            var StateString = BitConverter.ToString(byteArray);
            SomethingChanged?.Invoke(this, new CustomEventArgs(senderData + ": " + StateString));


        }


    }
}