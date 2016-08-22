using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParrotMiniDroneControle;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Tests
{
    [TestClass]
    public class ChannelTests
    {
        [TestMethod]
        public void DateArray()
        {
            var date = new DateTime(2016,06,10,10,06,16,200);
            var dateArray = DroneConnection.GetDateArray(date);
            var assumedArray = new byte[] { 0x32, 0x30, 0x31, 0x36, 0x2D, 0x30, 0x36, 0x2D, 0x31, 0x30, 0x00 };
            CollectionAssert.AreEqual(assumedArray, dateArray);
        }

        [TestMethod]
        public void TimeArray()
        {
            var date = new DateTime(2016, 06, 10, 10, 06, 16, 200);
            var dateArray = DroneConnection.GetTimeArray(date);
            var assumedArray = new byte[] { 0x54, 0x31, 0x30, 0x30, 0x36, 0x31, 0x36, 0x2B, 0x32, 0x30, 0x30, 0x00 };
            CollectionAssert.AreEqual(assumedArray, dateArray);
        }
        
    }
}
