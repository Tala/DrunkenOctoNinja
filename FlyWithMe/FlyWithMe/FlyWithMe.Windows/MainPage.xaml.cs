using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Enumeration;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace FlyWithMe
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            //Task.WaitAll(Stuff());
            Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => Stuff());

        }

        private async void Stuff()
        {
            DeviceInformation device = null;
            var serviceGuid = ParrotUuids.Service_C00;

            //Find the devices that expose the service  
            var devices = await DeviceInformation.FindAllAsync(GattDeviceService.GetDeviceSelectorFromUuid(serviceGuid));
            //var devices = await DeviceInformations.FindAllAsync(GattDeviceService.GetDeviceSelectorFromUuid(GattServiceUuids.GenericAccess));

            if (devices.Count == 0)
                return;

            foreach (var di in devices)
            {
                lstDevices.Items.Add(new MyBluetoothLEDevice(di));
                device = di;
            }

            //Connect to the service  
            var service = await GattDeviceService.FromIdAsync(device.Id);
            //if (service == null)
            //    return;

            //Obtain the characteristic we want to interact with
            //var characteristic = service.GetCharacteristics(ParrotUuids.Characteristic_A01)[0];
            ////Read the value  
            //var deviceNameBytes = (await characteristic.ReadValueAsync()).Value.ToArray();
            ////Convert to string  
            //var deviceName = Encoding.UTF8.GetString(deviceNameBytes, 0, deviceNameBytes.Length);

            //Get the accelerometer data characteristic  
            var serveces = service.GetIncludedServices(ParrotUuids.Characteristic_B01);
            var ids = serveces.Select(gattDeviceService => gattDeviceService.Uuid.ToString()).ToList();


            var accData1 = service.GetCharacteristics(ParrotUuids.Characteristic_C1)[0];
            

            //Subcribe value changed  
            //accData.ValueChanged += accData_ValueChanged;
            //Set configuration to notify  
            //await accData.WriteClientCharacteristicConfigurationDescriptorAsync(GattClientCharacteristicConfigurationDescriptorValue.Notify);
            ////Get the accelerometer configuration characteristic  
            //var accConfig = service.GetCharacteristics(new Guid("9a66fd21-0800-9191-11e4-012d1540cb8e"))[0];
            ////Write 1 to start accelerometer sensor  
            //await accConfig.WriteValueAsync(new byte[] { 1 }.AsBuffer());
        }

        async void accData_ValueChanged(GattCharacteristic sender, GattValueChangedEventArgs args)
        {
            var values = (await sender.ReadValueAsync()).Value.ToArray();
            var x = values[0];
            var y = values[1];
            var z = values[2];
        }

        #region Nested type: MyBluetoothLEDevice

        public class MyBluetoothLEDevice
        {
        public DeviceInformation BluetoothLEDevice { get; }

        public MyBluetoothLEDevice(DeviceInformation gattDeviceService)
            {

                BluetoothLEDevice = gattDeviceService;
            }

        public override string ToString()
            {
                return string.Format("{0} ({1})", BluetoothLEDevice.Name, BluetoothLEDevice.IsEnabled);
            }
        }

        #endregion
    }
}
