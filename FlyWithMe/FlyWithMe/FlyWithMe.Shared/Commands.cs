using System;
using System.Collections.Generic;
using System.Text;

namespace FlyWithMe
{
    public static class Commands
    {
        public static Command TakeOff = new Command(0x04, 0x00, 0x01, 0x00);
        public static Command Landing = new Command(0x04, 0x00, 0x03, 0x00);
        public static Command EmergencyShutdown = new Command(0x04, 0x00, 0x04, 0x00);
        public static Command Pitch = new Command(0x02, 0x00, 0x02, 0x02);
        public static Command Roll = new Command(0x02, 0x00, 0x02, 0x01);
        public static Command Yaw = new Command(0x02, 0x00, 0x02, 0x03);
        public static Command UpDownAkaGaz = new Command(0x02, 0x00, 0x02, 0x04);
    }


    public class Command
    {
        public byte FirstValue { get; }
        private short? argumentValue;
        public byte ProjectId { get; }
        public byte ClassId { get; }
        public byte CommandId { get; }
        public byte ArgumentId { get; }

        public short? ArgumentValue
        {
            get { return argumentValue; }
            set
            {
                if (value > 100 || value < -100)
                {
                    throw new ArgumentOutOfRangeException("Currently, Value should be larger than -100 and smaller than 100.");
                }
                argumentValue = value;
            }
        }

        public byte CommandCounter { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstValue"> currently 04 for general Commands (landing, takeoff) and 02 for motor commands (forwards, backwards...)</param>
        /// <param name="classId"></param>
        /// <param name="commandId"></param>
        /// <param name="argumentId"></param>
        /// <param name="argumentValue"></param>
        /// <param name="projectId"></param>
        public Command(byte firstValue, byte classId, byte commandId, byte argumentId, short? argumentValue = null, byte projectId = 0x02)
        {
            FirstValue = firstValue;
            ProjectId = projectId;
            ClassId = classId;
            CommandId = commandId;
            ArgumentId = argumentId;
            ArgumentValue = argumentValue;
        }

        public byte[] GetCommandBytes()
        {
            if (ArgumentValue == null)
            {
                return new[] { FirstValue, CommandCounter, ProjectId, ClassId, CommandId, ArgumentId };
            }
            return new[] { FirstValue, CommandCounter, ProjectId, ClassId, CommandId, ArgumentId, (byte) ArgumentValue };
        }
    }
    
}
