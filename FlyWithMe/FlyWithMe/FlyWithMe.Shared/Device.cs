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

        

        public Device()
        {
                    
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

        }
}