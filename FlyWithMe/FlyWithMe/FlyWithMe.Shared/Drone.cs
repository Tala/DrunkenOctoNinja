using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.ServiceModel;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Enumeration;

namespace FlyWithMe
{
    /// <summary>
    ///     base class for bluetooth drones
    /// </summary>
    public class Drone
    {
        public EventHandler<CustomEventArgs> SomethingChanged;

        public int motorCommandsCounter;
        public Channel simpleCommandsChannel;
        public int emergencyCommandsCounter;
        public string ProductId { get; }
        public string Name { get; }

        private TakeOff takeOff;
        private Landing landing;

        public NetworkAl Network { get; set; }

        public DroneState State { get; private set; }


        public async Task Initialize()
        {
            State = DroneState.Landed;
            motorCommandsCounter = 1;
            emergencyCommandsCounter = 1;
            Network = new NetworkAl();
            await Network.Initialize();
            Network.SomethingChanged += OnHandle_SomethingChanged;

        }

        public async Task Connect()
        {
            await Network.Connect();
            simpleCommandsChannel = new Channel(Network.GetServiceByGuid(Services.Command), ParrotUuids.Characteristic_A0B_SimpleCommands);
            takeOff = new TakeOff(simpleCommandsChannel);
            landing = new Landing(simpleCommandsChannel);
        }
        
        public async Task Disconnect()
        {
            await Network.Disconnect();
        }
        
        public async Task TakeOff()
        {
            // Wheels are on. 

            await takeOff.Execute();

            var customEventArgs = new CustomEventArgs("Gesendet: " + BitConverter.ToString(takeOff.DataBytes));
            SomethingChanged?.Invoke(this, customEventArgs);


        }

        public async Task Land()
        {

            await landing.Execute();

            var customEventArgs = new CustomEventArgs("Gesendet: " + BitConverter.ToString(landing.DataBytes));
            SomethingChanged?.Invoke(this, customEventArgs);
        }

        internal async Task Forward()
        {
            var pitch = Commands.Forward;
            while (pitch.Steps >= 0)
            {
                pitch.CommandCounter = (byte)motorCommandsCounter;
                await Network.SendData(
                    ParrotUuids.Service_A00,
                    ParrotUuids.Characteristic_A0A_Movement,
                    pitch);
                SomethingChanged?.Invoke(this, new CustomEventArgs("Gesendet: " + BitConverter.ToString(pitch.GetCommandBytes())));
                pitch.Steps--;
                motorCommandsCounter++;
            }


        }

        internal async Task Backward()
        {
            var pitch = Commands.Backward;
            while (pitch.Steps >= 0)
            {
                pitch.CommandCounter = (byte)motorCommandsCounter;
                await Network.SendData(
                    ParrotUuids.Service_A00,
                    ParrotUuids.Characteristic_A0A_Movement,
                    pitch);
                SomethingChanged?.Invoke(this, new CustomEventArgs("Gesendet: " + BitConverter.ToString(pitch.GetCommandBytes())));
                pitch.Steps--;
                motorCommandsCounter++;
            }

        }
        
        internal async Task Left()
        {
            var roll = Commands.Left;
            while (roll.Steps >= 0)
            {
                roll.CommandCounter = (byte)motorCommandsCounter;
                await Network.SendData(ParrotUuids.Service_A00, ParrotUuids.Characteristic_A0A_Movement, roll);
                SomethingChanged?.Invoke(this, new CustomEventArgs("Gesendet: " + BitConverter.ToString(roll.GetCommandBytes())));
                roll.Steps--;
                motorCommandsCounter++;
            }

        }

        internal async Task Right()
        {
            var roll = Commands.Right;
            while (roll.Steps >= 0)
            {
                roll.CommandCounter = (byte)motorCommandsCounter;
                await Network.SendData(ParrotUuids.Service_A00, ParrotUuids.Characteristic_A0A_Movement, roll);
                SomethingChanged?.Invoke(this, new CustomEventArgs("Gesendet: " + BitConverter.ToString(roll.GetCommandBytes())));
                roll.Steps--;
                motorCommandsCounter++;
            }

        }

        internal async Task Up()
        {
            var gaz = Commands.Up;
            while (gaz.Steps >= 0)
            {
                gaz.CommandCounter = (byte)motorCommandsCounter;
                await Network.SendData(ParrotUuids.Service_A00, ParrotUuids.Characteristic_A0A_Movement, gaz);
                var customEventArgs = new CustomEventArgs("Gesendet: " + BitConverter.ToString(gaz.GetCommandBytes()));
                SomethingChanged?.Invoke(this, customEventArgs);
                gaz.Steps--;
                motorCommandsCounter++;
            }

        }

        internal async Task Down()
        {
            var gaz = Commands.Down;
            while (gaz.Steps >= 0)
            {
                gaz.CommandCounter = (byte)motorCommandsCounter;
                await Network.SendData(ParrotUuids.Service_A00, ParrotUuids.Characteristic_A0A_Movement, gaz);
                SomethingChanged?.Invoke(this, new CustomEventArgs("Gesendet: " + BitConverter.ToString(gaz.GetCommandBytes())));
                gaz.Steps--;
                motorCommandsCounter++;
            }

        }

        internal async Task EmergencyStop()
        {
            var command = Commands.EmergencyShutdown;
            command.CommandCounter = (byte)emergencyCommandsCounter;
            await Network.SendData(ParrotUuids.Service_A00, ParrotUuids.Characteristic_A0C_EmergencyStop, command);
            SomethingChanged?.Invoke(this, new CustomEventArgs("Gesendet: " + BitConverter.ToString(command.GetCommandBytes())));
            emergencyCommandsCounter++;
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
        internal async Task FlipForward()
        {
            //    // await Stabilise();
            //    var flip = Commands.ForwardFlip;
            //    flip.CommandCounter = (byte)simpleCommandsCounter;
            //    await Network.SendData(ParrotUuids.Service_A00, ParrotUuids.Characteristic_A0B_SimpleCommands, flip);
            //    SomethingChanged?.Invoke(this, new CustomEventArgs("Gesendet: " + BitConverter.ToString(flip.GetCommandBytes())));
            //    simpleCommandsCounter++;
            //
        }

        public async Task Hover()
        {
            var hover = Commands.Hover;
            while (hover.Steps >= 0)
            {
                hover.CommandCounter = (byte)motorCommandsCounter;
                await Network.SendData(ParrotUuids.Service_A00, ParrotUuids.Characteristic_A0A_Movement, hover);
                SomethingChanged?.Invoke(this, new CustomEventArgs("Gesendet: " + BitConverter.ToString(hover.GetCommandBytes())));
                hover.Steps--;
                motorCommandsCounter++;
            }
        }
    }
}