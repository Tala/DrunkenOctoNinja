using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

        public NetworkAl()
        {
            DeviceInformations = new List<DeviceInformation>();
            DeviceServices = new List<GattDeviceService>();
            Characteristics = new Dictionary<string, List<GattCharacteristic>>();

        }

        public void Start()
        {
            RegisterEventhandling(ParrotUuids.Service_D21);
            RegisterEventhandling(ParrotUuids.Service_D51);
        }

        public void Stop()
        {
            DeregisterEventhandling(ParrotUuids.Service_D21);
            DeregisterEventhandling(ParrotUuids.Service_D51);
        }

        public void SendData(Guid servieGuid, Guid characteristicGuid, object data)
        {
            // get list of characteristics by serviceGuid
            var characteristicList = Characteristics[servieGuid.ToString()];
            // get characteristic
            var characteristic = characteristicList.FirstOrDefault(c => c.Uuid == characteristicGuid);
            // write value async
            Task.WaitAll(new Task(async () =>
            {
                // maybe add writing of descriptions before writing of values?
                // TODO Send correct data
                await characteristic.WriteValueAsync(new byte[] {1}.AsBuffer());
            }));
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
            
        }

        private void RegisterEventhandling(Guid serviceUuid)
        {
            var charachteristics = Characteristics[serviceUuid.ToString()];
            foreach (var characteristic in charachteristics)
            {
                characteristic.ValueChanged += Characteristic_ValueChanged;
            }

        }

        private void DeregisterEventhandling(Guid serviceUuid)
        {
            var charachteristics = Characteristics[serviceUuid.ToString()];
            foreach (var characteristic in charachteristics)
            {
                characteristic.ValueChanged -= Characteristic_ValueChanged;
            }

        }

        private void Characteristic_ValueChanged(GattCharacteristic sender, GattValueChangedEventArgs args)
        {


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
    }
}
