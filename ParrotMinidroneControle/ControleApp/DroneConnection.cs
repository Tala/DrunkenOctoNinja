using System;
using System.Globalization;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace ParrotMiniDroneControle
{
    /// <summary>
    ///     New implementation of NetworkAL
    ///     Contains channels where commands are send
    /// </summary>
    public class DroneConnection
    {
        public Channel MovementChannel { get; set; }
        public Channel EmergencyChannel { get; set; }
        public Channel SimpleChannel { get; private set; }

        public NetworkAl Network { get; set; }

        public async Task Initialise()
        {
            Network = new NetworkAl();
            await Network.Initialize();
        }

        public async Task Connect()
        {
            await Network.Connect();
            // init channels
            var characteristic = GetCharacteristic(Network.GetServiceByGuid(Services.Command),
                ParrotUuids.Characteristic_A0B_SimpleCommands);
            SimpleChannel = new Channel(characteristic);

            var characteristic1 = GetCharacteristic(Network.GetServiceByGuid(Services.Command),
                ParrotUuids.Characteristic_A0A_Movement);
            MovementChannel = new Channel(characteristic1);

            var characteristic2 = GetCharacteristic(Network.GetServiceByGuid(Services.Command),
                ParrotUuids.Characteristic_A0C_EmergencyStop);
            EmergencyChannel = new Channel(characteristic2);

            // send init commands/information on channels
            await InitMovementChannel();
        }

        private GattCharacteristic GetCharacteristic(GattDeviceService service, Guid characteristicId)
        {
            var characteristic = service.GetCharacteristics(characteristicId)[0];
            return characteristic;
        }

        public void Disconnect()
        {
            throw new NotImplementedException();
        }

        public async Task SendMovementCommand(MoveCommandBytes command)
        {
            await MovementChannel.SendData(command);
        }

        public async Task SendSimpleCommand(CommandBytes command)
        {
            await SimpleChannel.SendData(command);
        }

        public async Task SendEmergencyCommand(CommandBytes command)
        {
            await EmergencyChannel.SendData(command);
        }

        public async Task InitMovementChannel()
        {
            var date = DateTime.Now;
            var dateArray = GetDateArray(date);
            var timeArray = GetTimeArray(date);
            await MovementChannel.SendData(new CommonCommandBytes(0x01, dateArray));
            await MovementChannel.SendData(new CommonCommandBytes(0x02, timeArray));
        }

        /// <summary>
        ///     Transforms current date in byte array that can be directly send to the drone
        /// </summary>
        /// <returns>ByteArray in Form: 3X 3X 3X 3X 2D 3X 3X 2D 3X 3X 00 where X are taken from current Date</returns>
        public static byte[] GetDateArray(DateTime date)
        {
            var dateString = date.ToString("yyyy-MM-dd");
            var dateBytes = new byte[11];

            dateBytes[4] = 0x2D;
            dateBytes[7] = 0x2D;

            for (var n = 0; n < dateString.Length; n++)
            {
                if (n == 4 || n == 7) continue;
                byte.TryParse("3" + dateString[n], NumberStyles.HexNumber, null as IFormatProvider, out dateBytes[n]);
            }
            dateBytes[10] = 0x00;

            return dateBytes;
        }

        public static byte[] GetTimeArray(DateTime time)
        {
            var timeString = time.ToString("HHmmssffff");
            var timeArray = new byte[12];
            var tempArray = new byte[12];
            tempArray[0] = 0x54;

            for (var n = 0; n < timeString.Length; n++)
            {
                byte.TryParse("3" + timeString[n], NumberStyles.HexNumber, null as IFormatProvider, out tempArray[n + 1]);
            }
            tempArray.CopyTo(timeArray,0);
            timeArray[7] = 0x2B;
            timeArray[8] = tempArray[7];
            timeArray[9] = tempArray[8];
            timeArray[10] = tempArray[9];
            timeArray[11] = 0x00;
            return timeArray;
        }
    }
}