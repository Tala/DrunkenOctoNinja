using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Enumeration;

namespace FlyWithMe
{
    /// <summary>
    /// Implementation of Network communication for bluetooth between drone and app
    /// </summary>
    public class NetworkAl
    {
        public List<DeviceInformation> DeviceInformations { get; set; }

        public List<GattDeviceService> DeviceServices { get; }

        public Dictionary<string, List<GattCharacteristic>> Characteristics { get; }

        public string StateString { get; set; }

        public EventHandler<CustomEventArgs> SomethingChanged;

        public NetworkAl()
        {
            DeviceInformations = new List<DeviceInformation>();
            DeviceServices = new List<GattDeviceService>();
            Characteristics = new Dictionary<string, List<GattCharacteristic>>();

        }

        public async Task Disconnect()
        {
            await DeregisterEventhandling(ParrotUuids.Service_B00);
            await DeregisterEventhandling(ParrotUuids.Service_D21);
            await DeregisterEventhandling(ParrotUuids.Service_D51);
        }

        public async Task SendData(Guid servieGuid, Guid characteristicGuid, Command data)
        {
            // get list of characteristics by serviceGuid
            var characteristicList = Characteristics[servieGuid.ToString()];
            // get characteristic
            var characteristic = characteristicList.FirstOrDefault(c => c.Uuid == characteristicGuid);
            // write value async
            await characteristic.WriteValueAsync(data.GetCommandBytes().AsBuffer(), GattWriteOption.WriteWithoutResponse);
        }

        public void ReadData()
        {
            
        }

        public async Task Initialize()
        {
            var devices = await DeviceInformation.FindAllAsync(GattDeviceService.GetDeviceSelectorFromUuid(ParrotUuids.Service_A00));
            DeviceInformations.Add(devices[0]);
            devices = await DeviceInformation.FindAllAsync(GattDeviceService.GetDeviceSelectorFromUuid(ParrotUuids.Service_B00));
            DeviceInformations.Add(devices[0]);
            devices = await DeviceInformation.FindAllAsync(GattDeviceService.GetDeviceSelectorFromUuid(ParrotUuids.Service_C00));
            DeviceInformations.Add(devices[0]);
            devices = await DeviceInformation.FindAllAsync(GattDeviceService.GetDeviceSelectorFromUuid(ParrotUuids.Service_D21));
            DeviceInformations.Add(devices[0]);
            devices = await DeviceInformation.FindAllAsync(GattDeviceService.GetDeviceSelectorFromUuid(ParrotUuids.Service_D51));
            DeviceInformations.Add(devices[0]);
        }

        public async Task Connect()
        {
            // create service
            DeviceServices.Add(await GattDeviceService.FromIdAsync(DeviceInformations[0].Id));
            DeviceServices.Add(await GattDeviceService.FromIdAsync(DeviceInformations[1].Id));
            DeviceServices.Add(await GattDeviceService.FromIdAsync(DeviceInformations[2].Id));
            DeviceServices.Add(await GattDeviceService.FromIdAsync(DeviceInformations[3].Id));
            DeviceServices.Add(await GattDeviceService.FromIdAsync(DeviceInformations[4].Id));

            // register characteristics A00
            RegisterCharacteristic(ParrotUuids.Service_A00, ParrotUuids.Characteristic_A01);
            RegisterCharacteristic(ParrotUuids.Service_A00, ParrotUuids.Characteristic_Stop);
            RegisterCharacteristic(ParrotUuids.Service_A00, ParrotUuids.Characteristic_Movement);
            RegisterCharacteristic(ParrotUuids.Service_A00, ParrotUuids.Characteristic_TakeOffAndLand);
            RegisterCharacteristic(ParrotUuids.Service_A00, ParrotUuids.Characteristic_EmergencyStop);
            RegisterCharacteristic(ParrotUuids.Service_A00, ParrotUuids.Characteristic_InitCount1To20);
            RegisterCharacteristic(ParrotUuids.Service_A00, ParrotUuids.Characteristic_A1F);

            // register characteristics B00
            RegisterCharacteristic(ParrotUuids.Service_B00, ParrotUuids.Characteristic_B01);
            RegisterCharacteristic(ParrotUuids.Service_B00, ParrotUuids.Characteristic_B0Ebc_Bd);
            RegisterCharacteristic(ParrotUuids.Service_B00, ParrotUuids.Characteristic_B1Be3_E4);
            RegisterCharacteristic(ParrotUuids.Service_B00, ParrotUuids.Characteristic_B1Ce6_E7);
            RegisterCharacteristic(ParrotUuids.Service_B00, ParrotUuids.Characteristic_B1F);
            RegisterCharacteristic(ParrotUuids.Service_B00, ParrotUuids.Characteristic_Battery_B0Fbf_C0);

            // register characteristics C00
            RegisterCharacteristic(ParrotUuids.Service_C00, ParrotUuids.Characteristic_C1);

            // register characteristics D21
            RegisterCharacteristic(ParrotUuids.Service_D21, ParrotUuids.Characteristic_D22);
            RegisterCharacteristic(ParrotUuids.Service_D21, ParrotUuids.Characteristic_D23);
            RegisterCharacteristic(ParrotUuids.Service_D21, ParrotUuids.Characteristic_D24);
            
            // register characteristics D51
            RegisterCharacteristic(ParrotUuids.Service_D51, ParrotUuids.Characteristic_D52);
            RegisterCharacteristic(ParrotUuids.Service_D51, ParrotUuids.Characteristic_D53);
            RegisterCharacteristic(ParrotUuids.Service_D51, ParrotUuids.Characteristic_D54);

            await RegisterEventhandling(ParrotUuids.Service_B00);
            await RegisterEventhandling(ParrotUuids.Service_D21);
            await RegisterEventhandling(ParrotUuids.Service_D51);


            // get list of characteristics by serviceGuid
            var characteristicList = Characteristics[ParrotUuids.Service_A00.ToString()];
            // get characteristic
            var characteristic = characteristicList.FirstOrDefault(c => c.Uuid == ParrotUuids.Characteristic_InitCount1To20);

            for (int i = 0; i < 20; i++)
            {
                byte[] value = new byte[3];
                value[0] = (byte)(0x1);
                value[1] = (byte)(i + 1);
                value[2] = (byte)(i + 1);
                // write value async
                await characteristic.WriteValueAsync(value.AsBuffer(), GattWriteOption.WriteWithoutResponse);
                Task.WaitAll(Task.Delay(50));
            }
        }

        private async Task RegisterEventhandling(Guid serviceUuid)
        {
            var charachteristics = Characteristics[serviceUuid.ToString()];
            foreach (var characteristic in charachteristics)
            {
                characteristic.ValueChanged += Characteristic_ValueChanged;
                await characteristic.WriteClientCharacteristicConfigurationDescriptorAsync(GattClientCharacteristicConfigurationDescriptorValue.Notify);
            }

        }

        private async Task DeregisterEventhandling(Guid serviceUuid)
        {
            var charachteristics = Characteristics[serviceUuid.ToString()];
            foreach (var characteristic in charachteristics)
            {
                characteristic.ValueChanged -= Characteristic_ValueChanged;
                await characteristic.WriteClientCharacteristicConfigurationDescriptorAsync(GattClientCharacteristicConfigurationDescriptorValue.Notify);
                await Task.Delay(50);
            }

        }

        private async void Characteristic_ValueChanged(GattCharacteristic sender, GattValueChangedEventArgs args)
        {
            var senderData = sender.Uuid;
            var byteArray = args.CharacteristicValue.ToArray();
           StateString = Encoding.UTF8.GetString(byteArray, 0, byteArray.Length);
            SomethingChanged?.Invoke(this, new CustomEventArgs(StateString));
        }

        private void RegisterCharacteristic(Guid service_uuid, Guid characteristicUuid)
        {
            var accData = DeviceServices.FirstOrDefault(s => s.Uuid == service_uuid).GetCharacteristics(characteristicUuid)[0];

            if (!Characteristics.ContainsKey(service_uuid.ToString()))
            {
                Characteristics.Add(service_uuid.ToString(), new List<GattCharacteristic>());
            }
            Characteristics[service_uuid.ToString()].Add(accData);
        }


//        //Find the devices that expose the service  
//        var devices = await Windows.Devices.Enumeration.DeviceInformation.FindAllAsync(GattDeviceService.GetDeviceSelectorFromUuid(GattServiceUuids.GenericAccess));  
//       if (devices.Count==0)  
//         return;  
//       //Connect to the service  
//       var service = await GattDeviceService.FromIdAsync(devices[0].Id);  
//       if (service == null)  
//         return;  
//       //Obtain the characteristic we want to interact with  
//       var characteristic = service.GetCharacteristics(GattCharacteristic.ConvertShortIdToUuid(0x2A00))[0];
//        //Read the value  
//        var deviceNameBytes = (await characteristic.ReadValueAsync()).Value.ToArray();
//        //Convert to string  
//        var deviceName = Encoding.UTF8.GetString(deviceNameBytes, 0, deviceNameBytes.Length);

//        Now let's see how to interact with an non standard service and a characteristic that notifies when changing. I've chosen the accelerometer service of the SensorTAG:

//       //Find the devices that expose the service  
//        var devices = await Windows.Devices.Enumeration.DeviceInformation.FindAllAsync(GattDeviceService.GetDeviceSelectorFromUuid(new Guid("F000AA10-0451-4000-B000-000000000000")));  
//       if (devices.Count==0)  
//         return;  
//       //Connect to the service  
//       var accService = await GattDeviceService.FromIdAsync(devices[0].Id);  
//       if (accService == null)  
//         return;  
//       //Get the accelerometer data characteristic  
//       var accData = accService.GetCharacteristics(new Guid("F000AA11-0451-4000-B000-000000000000"))[0];
//        //Subcribe value changed  
//        accData.ValueChanged += accData_ValueChanged;  
//       //Set configuration to notify  
//       await accData.WriteClientCharacteristicConfigurationDescriptorAsync(GattClientCharacteristicConfigurationDescriptorValue.Notify);
//        //Get the accelerometer configuration characteristic  
//        var accConfig = accService.GetCharacteristics(new Guid("F000AA12-0451-4000-B000-000000000000"))[0];
//        //Write 1 to start accelerometer sensor  
//        await accConfig.WriteValueAsync((new byte[]{1}).AsBuffer());  

//In this case we connect to the service, we subscribe to the data characteristic notifications (we also ensure that the characteristic is in notify mode), and then we start the accelerometer sensor. Once the sensor starts it will notify us every 1000 ms. (default value that can be changed).

// async void accData_ValueChanged(GattCharacteristic sender, GattValueChangedEventArgs args)
//        {
//            var values = (await sender.ReadValueAsync()).Value.ToArray();
//            var x = values[0];
//            var y = values[1];
//            var z = values[2];
//        }
    }


    public class CustomEventArgs : EventArgs
    {
        public CustomEventArgs(string s)
        {
            msg = s;
        }
        private string msg;
        public string Message
        {
            get { return msg; }
        }
    }
}
