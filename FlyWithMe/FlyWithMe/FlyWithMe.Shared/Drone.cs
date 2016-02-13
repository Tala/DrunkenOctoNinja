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
        

        public async Task Initialize()
        {
            Network = new NetworkAl();
            await Network.Initialize();
        }

        public async Task Connect()
        {
            await Network.Connect();
        }



        public void Disconnect()
        {
            Network.Disconnect();
        }
        
        public void RecieveData()
        {
        }

        public void WaitForAnswer()
        {
        }

        internal void Forward()
        {
            var pitch = Commands.Pitch;
            pitch.ArgumentValue = 20;
            Network.SendData(ParrotUuids.Service_A00, ParrotUuids.Characteristic_Movement, pitch);
        }

        internal void Backward()
        {
            var pitch = Commands.Pitch;
            pitch.ArgumentValue = -20;
            Network.SendData(ParrotUuids.Service_A00, ParrotUuids.Characteristic_Movement, pitch);
        }

        internal void Left()
        {
            var roll = Commands.Roll;
            roll.ArgumentValue = -20;
            Network.SendData(ParrotUuids.Service_A00, ParrotUuids.Characteristic_Movement, roll);
        }

        internal void Right()
        {
            var roll = Commands.Roll;
            roll.ArgumentValue = 20;
            Network.SendData(ParrotUuids.Service_A00, ParrotUuids.Characteristic_Movement, roll);
        }

        internal void Up()
        {
            var gaz = Commands.UpDownAkaGaz;
            gaz.ArgumentValue = 20;
            Network.SendData(ParrotUuids.Service_A00, ParrotUuids.Characteristic_Movement, gaz);
        }

        internal void Down()
        {
            var gaz = Commands.UpDownAkaGaz;
            gaz.ArgumentValue = -20;
            Network.SendData(ParrotUuids.Service_A00, ParrotUuids.Characteristic_Movement, gaz);
        }

        public void TakeOff()
        {
            Network.SendData(ParrotUuids.Service_A00, ParrotUuids.Characteristic_TakeOffAndLand, Commands.TakeOff);
        }

        public void Land()
        {
            Network.SendData(ParrotUuids.Service_A00, ParrotUuids.Characteristic_TakeOffAndLand, Commands.Landing);
        }

        internal void EmergencyStop()
        {
            Network.SendData(ParrotUuids.Service_A00, ParrotUuids.Characteristic_EmergencyStop, Commands.EmergencyShutdown);
        }
    }
}