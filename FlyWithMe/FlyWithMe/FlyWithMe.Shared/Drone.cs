using System;
using System.Collections.Generic;
using System.Linq;
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
        public string ProductId { get; }
        public string Name { get; }

        public NetworkAl Network { get; set; }
        public EventHandler SomethingChanged;

        public int motorCommandsCounter;
        public int simpleCommandsCounter;


        public async Task Initialize()
        {
            motorCommandsCounter = 1;
            simpleCommandsCounter = 1;
            Network = new NetworkAl();
            await Network.Initialize();
        }

        public async Task Connect()
        {
            await Network.Connect();
        }



        public async Task Disconnect()
        {
            await Network.Disconnect();
        }
        
        public void RecieveData()
        {
        }

        public void WaitForAnswer()
        {
        }

        internal async Task Forward()
        {
            var pitch = Commands.Pitch;
            pitch.CommandCounter = (byte)motorCommandsCounter;
            pitch.ArgumentValue = 20;
            await Network.SendData(ParrotUuids.Service_A00, ParrotUuids.Characteristic_Movement, pitch);
            motorCommandsCounter++;

        }

        internal async Task Backward()
        {
            var pitch = Commands.Pitch;
            pitch.CommandCounter = (byte)motorCommandsCounter;
            pitch.ArgumentValue = -20;
            await Network.SendData(ParrotUuids.Service_A00, ParrotUuids.Characteristic_Movement, pitch);
            motorCommandsCounter++;
        }

        internal async Task Left()
        {
            var roll = Commands.Roll;
            roll.CommandCounter = (byte)motorCommandsCounter;
            roll.ArgumentValue = -20;
            await Network.SendData(ParrotUuids.Service_A00, ParrotUuids.Characteristic_Movement, roll);
            motorCommandsCounter++;
        }

        internal async Task Right()
        {
            var roll = Commands.Roll;
            roll.CommandCounter = (byte)motorCommandsCounter;
            roll.ArgumentValue = 20;
            await Network.SendData(ParrotUuids.Service_A00, ParrotUuids.Characteristic_Movement, roll);
            motorCommandsCounter++;
        }

        internal async Task Up()
        {
            var gaz = Commands.UpDownAkaGaz;
            gaz.CommandCounter = (byte)motorCommandsCounter;
            gaz.ArgumentValue = 20;
            await Network.SendData(ParrotUuids.Service_A00, ParrotUuids.Characteristic_Movement, gaz);
            motorCommandsCounter++;
        }

        internal async Task Down()
        {
            var gaz = Commands.UpDownAkaGaz;
            gaz.CommandCounter = (byte)motorCommandsCounter;
            gaz.ArgumentValue = -20;
            await Network.SendData(ParrotUuids.Service_A00, ParrotUuids.Characteristic_Movement, gaz);
            motorCommandsCounter++;
        }

        public async Task TakeOff()
        {
            var command = Commands.TakeOff;
            command.CommandCounter = (byte)simpleCommandsCounter;
            await Network.SendData(ParrotUuids.Service_A00, ParrotUuids.Characteristic_TakeOffAndLand, command);
            simpleCommandsCounter++;
        }

        public async Task Land()
        {
            var command = Commands.Landing;
            command.CommandCounter = (byte)simpleCommandsCounter;
            await Network.SendData(ParrotUuids.Service_A00, ParrotUuids.Characteristic_TakeOffAndLand, command);
            simpleCommandsCounter++;
        }

        internal async Task EmergencyStop()
        {
            var command = Commands.EmergencyShutdown;
            command.CommandCounter = (byte)simpleCommandsCounter;
            await Network.SendData(ParrotUuids.Service_A00, ParrotUuids.Characteristic_EmergencyStop, command);
            simpleCommandsCounter++;
        }
    }
}