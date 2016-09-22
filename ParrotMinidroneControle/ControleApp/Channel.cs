using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Foundation;

namespace ParrotMiniDroneControle
{
    public class Channel
    {
        private int _counter = 1;
        private readonly GattCharacteristic Characteristic;

        public Channel(GattCharacteristic characteristic)
        {
            Characteristic = characteristic;
        }

        public async Task SendData(CommandBytes data)
        {
            data.CommandCounter = (byte) _counter;
            var buffer = data.GetCommandBytes().AsBuffer();
            // TODO: needs to be updated to current UWP10 api usage
            GattCommunicationStatus status = await Characteristic.WriteValueAsync(buffer, GattWriteOption.WriteWithoutResponse);
            _counter++;
            Log.Instance.LogCommandToDrone(typeof(Channel) + "",data.GetCommandBytes());
        }
    }
}