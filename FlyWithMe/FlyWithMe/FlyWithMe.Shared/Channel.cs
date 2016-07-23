using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace FlyWithMe
{
    public class Channel
    {
        private GattCharacteristic Characteristic;
        private int _counter = 1;

        public Channel(GattCharacteristic characteristic)
        {
            Characteristic = characteristic;
        }

        public Channel(GattDeviceService service, Guid characteristicId)
        {
            Characteristic = service.GetCharacteristics(characteristicId)[0];
        }

        public async Task SendData(CommandBytes data)
        {
            data.CommandCounter = (byte)_counter;
            var buffer = data.GetCommandBytes().AsBuffer();
            GattCommunicationStatus status = await Characteristic.WriteValueAsync(buffer, GattWriteOption.WriteWithoutResponse);
            _counter++;
        }

    }
}