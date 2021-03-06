﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using ParrotMiniDroneControle;

namespace Tests
{
    [TestClass]
    public class CommandsTests
    {
        [TestMethod]
        public void TakeOff()
        {
            var command = Commands.TakeOff;
            var bytes = command.GetCommandBytes();
            CollectionAssert.AreEqual(new byte[] { 04, 00, 02, 00, 01, 00 }, bytes);
        }

        [TestMethod]
        public void Land()
        {
            var command = Commands.Landing;
            var bytes = command.GetCommandBytes();
            CollectionAssert.AreEqual(new byte[] { 04, 00, 02, 00, 03, 00 }, bytes);
        }

        [TestMethod]
        public void FlipCommand_Forward()
        {
            var command = Commands.ForwardFlip;
            var bytes = command.GetCommandBytes();
            CollectionAssert.AreEqual(new byte[] { 04, 00, 02, 04, 00, 00, 00, 00, 00, 00 }, bytes);
        }

        [TestMethod]
        public void FlipCommand_Backwards()
        {
            var command = Commands.BackwardsFlip;
            var bytes = command.GetCommandBytes();
            CollectionAssert.AreEqual(new byte[] { 04, 00, 02, 04, 00, 00, 01, 00, 00, 00 }, bytes);
        }

        [TestMethod]
        public void FlipCommand_Right()
        {
            var command = Commands.RightFlip;
            var bytes = command.GetCommandBytes();
            CollectionAssert.AreEqual(new byte[] { 04, 00, 02, 04, 00, 00, 02, 00, 00, 00 }, bytes);
        }

        [TestMethod]
        public void FlipCommand_Left()
        {
            var command = Commands.LeftFlip;
            var bytes = command.GetCommandBytes();
            CollectionAssert.AreEqual(new byte[] { 04, 00, 02, 04, 00, 00, 03, 00, 00, 00 }, bytes);
        }

    }
}
