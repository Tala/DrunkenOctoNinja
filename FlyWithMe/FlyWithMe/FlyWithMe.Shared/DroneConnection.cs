using System;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace FlyWithMe
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
            var dateArray = GetDateArray();
            var timeArray = GetTimeArray();
            await MovementChannel.SendData(new CommonCommandBytes(0x01, dateArray));
            await MovementChannel.SendData(new CommonCommandBytes(0x02, timeArray));
        }

        /// <summary>
        ///     Transforms current date in byte array that can be directly send to the drone
        /// </summary>
        /// <returns>ByteArray in Form: 3X 3X 3X 3X 2D 3X 3X 2D 3X 3X where X are taken from current Date</returns>
        public static byte[] GetDateArray()
        {
            var date = DateTime.Now.ToString("yyyy-MM-dd");
            var dateBytes = new byte[11];

            dateBytes[4] = 0x2D;
            dateBytes[7] = 0x2D;

            for (var n = 0; n < date.Length; n++)
            {
                if (n == 4 || n == 7) continue;
                byte.TryParse("3" + date[n], out dateBytes[n]);
            }
            dateBytes[10] = 0x00;

            return dateBytes;
        }

        public static byte[] GetTimeArray()
        {
            var time = DateTime.Now.ToString("HHmmss");
            var timeArray = new byte[13];
            timeArray[0] = 0x54;

            for (var n = 0; n < time.Length; n++)
            {
                if (n == 4 || n == 7) continue;
                byte.TryParse("3" + time[n], out timeArray[n + 1]);
            }
            timeArray[7] = 0x2B;
            timeArray[8] = 0x30;
            timeArray[9] = 0x31;
            timeArray[10] = 0x30;
            timeArray[11] = 0x30;
            timeArray[12] = 0x00;
            return timeArray;
        }
    }
}