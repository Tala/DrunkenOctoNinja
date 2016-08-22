using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlyWithMe;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Tests
{
    [TestClass]
    class ChannelTests
    {
        [TestMethod]
        public void DateArray()
        {
            var date = new DateTime(2016,06,10,10,06,16,200);
            var dateArray = DroneConnection.GetDateArray(date);
            var assumedArray = new byte[] { 0x32, 0x30, 0x31, 0x36, 0x2D, 0x30, 0x36, 0x2D, 0x31, 0x30 };
            CollectionAssert.AreEqual(assumedArray, dateArray);
        }

        /* public async Task InitMovementChannel()
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
        }*/
    }
}
