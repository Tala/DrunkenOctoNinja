using System;
using System.Collections.Generic;
using System.Text;

namespace FlyWithMe
{
    public static class Commands
    {
        public static Command TakeOff = new Command(0x00, 0x01, 0x00);
        public static Command Landing = new Command(0x00, 0x03, 0x00);
        public static Command EmergencyShutdown = new Command(0x00, 0x04, 0x00);
        public static Command Pitch = new Command(0x00, 0x02, 0x02);
        public static Command Roll = new Command(0x00, 0x02, 0x01);
        public static Command Yaw = new Command(0x00, 0x02, 0x03);
        public static Command UpDownAkaGaz = new Command(0x00, 0x02, 0x04);
    }


    public class Command
    {
        private ushort? argumentValue;
        public byte ProjectId { get; }
        public byte ClassId { get; }
        public byte CommandId { get; }
        public byte ArgumentId { get; }

        public ushort? ArgumentValue
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

        public Command(byte classId, byte commandId, byte argumentId, ushort? argumentValue = null, byte projectId = 0x02)
        {
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
                return new[] { ProjectId, ClassId, CommandId, ArgumentId };
            }
            return new[] { ProjectId, ClassId, CommandId, ArgumentId, (byte) ArgumentValue };
        }
    }
    
}
