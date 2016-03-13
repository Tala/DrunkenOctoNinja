using System;

namespace FlyWithMe
{
    public static class Commands
    {
        public static Command TakeOff = new Command(0x04, 0x00, 0x01, 0x00);
        public static Command Landing = new Command(0x04, 0x00, 0x03, 0x00);
        public static Command EmergencyShutdown = new Command(0x04, 0x00, 0x04, 0x00);
        public static Command FlatTrim = new Command(0x02, 0x00, 0x00, 0x00);

        public static MoveCommand Up = new MoveCommand(0, 0, 0, 50, 5);
        public static MoveCommand Down = new MoveCommand(0, 0, 0, -50, 5);
        public static MoveCommand Forward = new MoveCommand(0, 50, 0, 0, 2);
        public static MoveCommand Backward = new MoveCommand(0, -50, 0, 0, 2);
        public static MoveCommand Right = new MoveCommand(50, 0, 0, 0, 2);
        public static MoveCommand Left = new MoveCommand(-50, 0, 0, 0, 2);
        public static MoveCommand TurnRight = new MoveCommand(0, 0, 50, 0, 2);
        public static MoveCommand TurnLeft = new MoveCommand(0, 0, -50, 0, 2);
        public static MoveCommand Hover = new MoveCommand(0, 0, 0, 0, 1);

        public static WheelCommand WheelOn = new WheelCommand(true);
        public static WheelCommand WheelOff = new WheelCommand(false);


        public static FlipCommand ForwardFlip = new FlipCommand(Direction.Front);
        public static FlipCommand BackwardsFlip = new FlipCommand(Direction.Back);
        public static FlipCommand RightFlip = new FlipCommand(Direction.Right);
        public static FlipCommand LeftFlip = new FlipCommand(Direction.Left);
    }

    public class FlipCommand : Command
    {
        public Direction direction { get; }

        public FlipCommand(Direction direction) : base(2,4,0,0)
        {
            this.direction = direction;
        }

        public new byte[] GetCommandBytes()
        {

            return new[] { FirstValue, CommandCounter, ProjectId, ClassId, CommandId, ArgumentId, (byte) direction, (byte)0, (byte)0, (byte)0 };
        }
    }

    public enum Direction
    {
        Front = 0x00,
        Back = 0x01,
        Right = 0x02,
        Left = 0x03
    }

    public class WheelCommand : Command
    {
        public byte WheelOn { get; set; }
        public WheelCommand(bool wheelOn) : base(2, 1, 2, 0, 2)
        {
            WheelOn = BitConverter.GetBytes(wheelOn)[0];
        }

        public new byte[] GetCommandBytes()
        {
            return new[] { FirstValue, CommandCounter, ProjectId, ClassId, CommandId, ArgumentId, WheelOn };
        }
    }


    public class MoveCommand: Command
    {
        public byte Roll { get; }
        public byte Pitch { get; }
        public byte Yaw { get; }
        public byte Up { get; }
        public int Steps { get; set; }

        public MoveCommand(short roll, short pitch, short yaw, short up, int steps) : base(2, 0, 2, 0)
        {
            Roll = ConvertToByte(roll);
            Pitch = ConvertToByte(pitch);
            Yaw = ConvertToByte(yaw);
            Up = ConvertToByte(up);
            Steps = steps;
        }

        public new byte[] GetCommandBytes()
        {
            var retval = new Byte[13];
            retval[0] = FirstValue;
            retval[1] = CommandCounter;
            retval[2] = ProjectId;
            retval[3] = ClassId;
            retval[4] = CommandId;
            retval[5] = ArgumentId;
            retval[6] = BitConverter.GetBytes(Steps > 0)[0];
            retval[7] = Roll;
            retval[8] = Pitch;
            retval[9] = Yaw;
            retval[10] = Up;
            return retval;
        }

        private byte ConvertToByte(short value)
        {
            var test = BitConverter.IsLittleEndian;
            return BitConverter.GetBytes(value)[0];
        }
    }

    public class Command
    {
        public byte ClassId { get; }
        public byte FirstValue { get; }
        public byte ProjectId { get; }
        public byte CommandId { get; }
        public byte ArgumentId { get; }


        public byte CommandCounter { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstValue"> currently 04 for general Commands (landing, takeoff) and 02 for motor commands (forwards, backwards...)</param>
        /// <param name="classId"></param>
        /// <param name="commandId"></param>
        /// <param name="argumentId"></param>
        /// <param name="projectId"></param>
        public Command(byte firstValue, byte classId, byte commandId, byte argumentId, byte projectId = 0x02)
        {
            FirstValue = firstValue;
            ProjectId = projectId;
            ClassId = classId;
            CommandId = commandId;
            ArgumentId = argumentId;
        }

        public byte[] GetCommandBytes()
        {
                return new[] { FirstValue, CommandCounter, ProjectId, ClassId, CommandId, ArgumentId };
        }
    }
    
}
