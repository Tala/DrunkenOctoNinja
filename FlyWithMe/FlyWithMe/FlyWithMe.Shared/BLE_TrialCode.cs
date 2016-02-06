using System;
using System.Collections.Generic;
using System.Text;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Enumeration;

namespace FlyWithMe
{
    public delegate void DeviceConnectionUpdatedHandler(bool isConnected);


    class BLE_TrialCode
    {

        public event DeviceConnectionUpdatedHandler DeviceConnectionUpdated;
    }
}
