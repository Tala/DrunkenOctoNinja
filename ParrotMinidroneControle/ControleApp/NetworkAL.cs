using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Enumeration;

namespace ParrotMiniDroneControle
{
    /// <summary>
    ///     Implementation of Network communication for bluetooth between drone and app
    /// </summary>
    public class NetworkAl
    {
        public EventHandler<GattValueChangedEventArgs> SomethingChanged;

        public NetworkAl()
        {
            DeviceInformations = new List<DeviceInformation>();
            DeviceServices = new List<GattDeviceService>();
            Characteristics = new Dictionary<string, List<GattCharacteristic>>();
        }

        public List<DeviceInformation> DeviceInformations { get; set; }

        public List<GattDeviceService> DeviceServices { get; }

        public Dictionary<string, List<GattCharacteristic>> Characteristics { get; }

        public string StateString { get; set; }

        public GattDeviceService GetServiceByGuid(Guid serviceId)
        {
            return DeviceServices.FirstOrDefault(s => s.Uuid == serviceId);
        }

        public async Task Disconnect()
        {
            await DeregisterEventhandling(ParrotUuids.Service_B00);
            await DeregisterEventhandling(ParrotUuids.Service_D21);
            await DeregisterEventhandling(ParrotUuids.Service_D51);
        }

        public async Task SendData(Guid service, Guid characteristicGuid, CommandBytes data)
        {
            var characteristicList = Characteristics[service.ToString()];
            var firstOrDefault = characteristicList.FirstOrDefault(c => c.Uuid == characteristicGuid);

            var buffer = data.GetCommandBytes().AsBuffer();
            var status = await firstOrDefault.WriteValueAsync(buffer, GattWriteOption.WriteWithoutResponse);

            Debug.WriteLine(status.ToString());
        }

        public void ReadData()
        {
        }

        public async Task Initialize()
        {
            var devices =
                await
                    DeviceInformation.FindAllAsync(GattDeviceService.GetDeviceSelectorFromUuid(ParrotUuids.Service_A00));
            DeviceInformations.Add(devices[0]);
            devices =
                await
                    DeviceInformation.FindAllAsync(GattDeviceService.GetDeviceSelectorFromUuid(ParrotUuids.Service_B00));
            DeviceInformations.Add(devices[0]);
            devices =
                await
                    DeviceInformation.FindAllAsync(GattDeviceService.GetDeviceSelectorFromUuid(ParrotUuids.Service_C00));
            DeviceInformations.Add(devices[0]);
            devices =
                await
                    DeviceInformation.FindAllAsync(GattDeviceService.GetDeviceSelectorFromUuid(ParrotUuids.Service_D21));
            DeviceInformations.Add(devices[0]);
            devices =
                await
                    DeviceInformation.FindAllAsync(GattDeviceService.GetDeviceSelectorFromUuid(ParrotUuids.Service_D51));
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
            RegisterCharacteristic(ParrotUuids.Service_A00, ParrotUuids.Characteristic_A02);
            RegisterCharacteristic(ParrotUuids.Service_A00, ParrotUuids.Characteristic_A0A_Movement);
            RegisterCharacteristic(ParrotUuids.Service_A00, ParrotUuids.Characteristic_A0B_SimpleCommands);
            RegisterCharacteristic(ParrotUuids.Service_A00, ParrotUuids.Characteristic_A0C_EmergencyStop);
            RegisterCharacteristic(ParrotUuids.Service_A00, ParrotUuids.Characteristic_A1E_InitCount1To20);
            RegisterCharacteristic(ParrotUuids.Service_A00, ParrotUuids.Characteristic_A1F);

            // register characteristics B00
            RegisterCharacteristic(ParrotUuids.Service_B00, ParrotUuids.Characteristic_B01);
            RegisterCharacteristic(ParrotUuids.Service_B00, ParrotUuids.Characteristic_B0E_DroneState);
            RegisterCharacteristic(ParrotUuids.Service_B00, ParrotUuids.Characteristic_B1B);
            RegisterCharacteristic(ParrotUuids.Service_B00, ParrotUuids.Characteristic_B1C);
            RegisterCharacteristic(ParrotUuids.Service_B00, ParrotUuids.Characteristic_B1F);
            RegisterCharacteristic(ParrotUuids.Service_B00, ParrotUuids.Characteristic_B0F_Battery);

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

            //await InitChannelA1E();
        }

        public async Task InitChannelA1E()
        {
            // get list of characteristics by serviceGuid
            var characteristicList = Characteristics[ParrotUuids.Service_A00.ToString()];
            // get characteristic
            var characteristic =
                characteristicList.FirstOrDefault(c => c.Uuid == ParrotUuids.Characteristic_A1E_InitCount1To20);

            for (var i = 0; i < 20; i++)
            {
                var value = new byte[3];
                value[0] = 0x1;
                value[1] = (byte) (i + 1);
                value[2] = (byte) (i + 1);
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
                await
                    characteristic.WriteClientCharacteristicConfigurationDescriptorAsync(
                        GattClientCharacteristicConfigurationDescriptorValue.Notify);
            }
        }

        private async Task DeregisterEventhandling(Guid serviceUuid)
        {
            var charachteristics = Characteristics[serviceUuid.ToString()];
            foreach (var characteristic in charachteristics)
            {
                characteristic.ValueChanged -= Characteristic_ValueChanged;
                await
                    characteristic.WriteClientCharacteristicConfigurationDescriptorAsync(
                        GattClientCharacteristicConfigurationDescriptorValue.Notify);
                await Task.Delay(50);
            }
        }

        private async void Characteristic_ValueChanged(GattCharacteristic sender, GattValueChangedEventArgs args)
        {
            SomethingChanged?.Invoke(sender, args);
        }

        private void RegisterCharacteristic(Guid service_uuid, Guid characteristicUuid)
        {
            var accData = DeviceServices
                .FirstOrDefault(s => s.Uuid == service_uuid)
                .GetCharacteristics(characteristicUuid)[0];

            if (!Characteristics.ContainsKey(service_uuid.ToString()))
            {
                Characteristics.Add(service_uuid.ToString(), new List<GattCharacteristic>());
            }
            Characteristics[service_uuid.ToString()].Add(accData);
        }
    }


    public class CustomEventArgs : EventArgs
    {
        public CustomEventArgs(string s)
        {
            Message = s;
        }

        public string Message { get; }
    }
}