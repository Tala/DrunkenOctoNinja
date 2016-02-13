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
    public class Device
    {
        public string ProductId { get; }
        public string Name { get; }

        public NetworkAl Network { get; set; }
        

        public async Task Initialize()
        {
            Network = new NetworkAl();
            await Network.Initialize();
        }

        public void Connect()
        {
            
        }



        public void Disconnect()
        {
        }

        public void SendData()
        {
        }

        public void RecieveData()
        {
        }

        public void WaitForAnswer()
        {
        }

        internal void Forward()
        {
            throw new NotImplementedException();
        }

        internal void Backward()
        {
            throw new NotImplementedException();
        }

        internal void Left()
        {
            throw new NotImplementedException();
        }

        internal void Right()
        {
            throw new NotImplementedException();
        }

        internal void Up()
        {
            throw new NotImplementedException();
        }

        internal void Down()
        {
            throw new NotImplementedException();
        }
    }
}