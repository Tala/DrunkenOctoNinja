using System;
using System.Collections.ObjectModel;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Enumeration;

namespace ParrotMiniDroneControle
{
    internal class DroneDiscovery
    {
        private static readonly Guid IoServiceUuid = new Guid("00004F0E-1212-efde-1523-785feabcd123");
        private static readonly Guid OutputCommandCharacteristicGuid = new Guid("00001565-1212-efde-1523-785feabcd123");

        private DeviceWatcher deviceWatcher;
        private DeviceWatcher gattServiceWatcher;
        private GattCharacteristic outputCommandCharacteristic;
        private GattDeviceService weDoIoService;

        public DroneDiscovery()
        {
            ResultCollection = new ObservableCollection<DeviceInformation>();
        }

        // Used to display list of available devices 
        public ObservableCollection<DeviceInformation> ResultCollection { get; }

        public void StopWatching()
        {
            if (null != deviceWatcher)
            {
                if (DeviceWatcherStatus.Started == deviceWatcher.Status ||
                    DeviceWatcherStatus.EnumerationCompleted == deviceWatcher.Status)
                {
                    deviceWatcher.Stop();
                }
            }
        }

        public void StartWatching()
        {
            //Reset displayed results
            ResultCollection.Clear();

            // Request additional properties
            string[] requestedProperties = {"System.Devices.Aep.DeviceAddress", "System.Devices.Aep.IsConnected"};

            // Add filter to watcher to look only for parrot mini drones
            deviceWatcher =
                DeviceInformation.CreateWatcher(
                    "(System.Devices.Aep.ProtocolId:=\"{e0cbf06c-cd8b-4647-bb8a-263b43f0f974}\")",
                    requestedProperties,
                    DeviceInformationKind.AssociationEndpoint);

            // Hook up handlers for the watcher events before starting the watcher
            deviceWatcher.Added += (watcher, deviceInfo) => { ResultCollection.Add(deviceInfo); };

            deviceWatcher.Updated += (watcher, deviceInfoUpdate) =>
            {
                foreach (var bleInfoDisp in ResultCollection)
                {
                    if (bleInfoDisp.Id == deviceInfoUpdate.Id)
                    {
                        bleInfoDisp.Update(deviceInfoUpdate);
                        break;
                    }
                }
            };

            deviceWatcher.EnumerationCompleted += (watcher, obj) =>
            {
                //await rootPage.Dispatcher.RunAsync(CoreDispatcherPriority.Low, () =>
                //{
                //    rootPage.NotifyUser(
                //        String.Format("{0} devices found. Enumeration completed. Watching for updates...", ResultCollection.Count),
                //        NotifyType.StatusMessage);
                //});
            };

            deviceWatcher.Removed += (watcher, deviceInfoUpdate) =>
            {
                // Find the corresponding DeviceInformation in the collection and remove it
                foreach (var bleInfoDisp in ResultCollection)
                {
                    if (bleInfoDisp.Id == deviceInfoUpdate.Id)
                    {
                        ResultCollection.Remove(bleInfoDisp);
                        break;
                    }
                }
            };

            deviceWatcher.Stopped += (watcher, obj) => { };

            deviceWatcher.Start();
        }

        private async void StartGattServiceWatcher(DeviceInformation deviceInfoDisp)
        {
            //Get the Bluetooth address for filtering the watcher
            var bleDevice = await BluetoothLEDevice.FromIdAsync(deviceInfoDisp.Id);
            var selector = "(" + GattDeviceService.GetDeviceSelectorFromUuid(IoServiceUuid) + ")"
                           + " AND (System.DeviceInterface.Bluetooth.DeviceAddress:=\""
                           + bleDevice.BluetoothAddress.ToString("X") + "\")";

            gattServiceWatcher = DeviceInformation.CreateWatcher(selector);

            // Hook up handlers for the watcher events before starting the watcher
            gattServiceWatcher.Added += async (watcher, deviceInfo) =>
            {
                weDoIoService = await GattDeviceService.FromIdAsync(deviceInfo.Id);
                outputCommandCharacteristic = weDoIoService.GetCharacteristics(OutputCommandCharacteristicGuid)[0];
            };

            //gattServiceWatcher.Updated += new TypedEventHandler<DeviceWatcher, DeviceInformationUpdate>(async (watcher, deviceInfoUpdate) => );

            //gattServiceWatcher.EnumerationCompleted += new TypedEventHandler<DeviceWatcher, Object>(async (watcher, obj) => );

            //gattServiceWatcher.Removed += new TypedEventHandler<DeviceWatcher, DeviceInformationUpdate>(async (watcher, deviceInfoUpdate) => );

            //gattServiceWatcher.Stopped += new TypedEventHandler<DeviceWatcher, Object>(async (watcher, obj) => );

            gattServiceWatcher.Start();
        }

        private async void PairDevice(DeviceInformation deviceInfoDisp)
        {
            var result = await deviceInfoDisp.Pairing.PairAsync();
            //result.Status == DevicePairingResultStatus.Paired 
        }

        private async void UnpairDevice(DeviceInformation deviceInfoDisp)
        {
            var result = await deviceInfoDisp.Pairing.UnpairAsync();
            // dupr.Status == DeviceUnpairingResultStatus.Unpaired
        }

        /* DataWriter writer = new DataWriter();
            byte[] data = new byte[] 
                {
                    1, // connectId - 1/2
                    1, // commandId - 1 is for writing motor power
                    1, // data size in bytes
                    100  // data, in this case - motor power - (100) full speed forward 
                };
            writer.WriteBytes(data);

            GattCommunicationStatus status = await outputCommandCharacteristic.WriteValueAsync(
                writer.DetachBuffer()); */
    }
}