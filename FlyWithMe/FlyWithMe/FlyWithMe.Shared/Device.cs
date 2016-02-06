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
        public Guid DeviceUuid { get; }

        public List<DeviceInformation> DeviceInformations { get; set; }

        public List<GattDeviceService> DeviceServices { get; private set; }

        public Dictionary<string,GattCharacteristic> Characteristics { get; }

        public Device()
        {
            DeviceInformations = new List<DeviceInformation>();
            DeviceServices = new List<GattDeviceService>();
            Characteristics = new Dictionary<string, GattCharacteristic>();
            Task.WaitAll(new Task(async delegate
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
            }));
            
        }

        public async void Connect()
        {
            // create service
            DeviceServices.Add(await GattDeviceService.FromIdAsync(ParrotUuids.Service_A00.ToString()));
            DeviceServices.Add(await GattDeviceService.FromIdAsync(ParrotUuids.Service_B00.ToString()));
            DeviceServices.Add(await GattDeviceService.FromIdAsync(ParrotUuids.Service_C00.ToString()));
            DeviceServices.Add(await GattDeviceService.FromIdAsync(ParrotUuids.Service_D21.ToString()));
            DeviceServices.Add(await GattDeviceService.FromIdAsync(ParrotUuids.Service_D51.ToString()));
            
            // register characteristics A00
            RegisterCharacteristic(ParrotUuids.Service_A00, ParrotUuids.Characteristic_A01);
            RegisterCharacteristic(ParrotUuids.Service_A00, ParrotUuids.Characteristic_Stop);
            RegisterCharacteristic(ParrotUuids.Service_A00, ParrotUuids.Characteristic_PowerMotors);
            RegisterCharacteristic(ParrotUuids.Service_A00, ParrotUuids.Characteristic_DateTime);
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

        private void RegisterCharacteristic(Guid service_uuid, Guid characteristicUuid)
        {
            var accData = DeviceServices.FirstOrDefault(s => s.Uuid == service_uuid).GetCharacteristics(characteristicUuid)[0];
            Characteristics.Add(service_uuid.ToString(), accData);
        }
    }
}