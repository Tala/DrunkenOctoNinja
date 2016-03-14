using System;
using System.Threading.Tasks;

namespace FlyWithMe
{
    public static class Commands
    {
        public static CommandBytes TakeOff = new CommandBytes(0x04, 0x00, 0x01, 0x00);
        public static CommandBytes Landing = new CommandBytes(0x04, 0x00, 0x03, 0x00);
        public static CommandBytes EmergencyShutdown = new CommandBytes(0x04, 0x00, 0x04, 0x00);
        public static CommandBytes FlatTrim = new CommandBytes(0x02, 0x00, 0x00, 0x00);

        public static MoveCommandBytes Up = new MoveCommandBytes(0, 0, 0, 50, 5);
        public static MoveCommandBytes Down = new MoveCommandBytes(0, 0, 0, -50, 5);
        public static MoveCommandBytes Forward = new MoveCommandBytes(0, 50, 0, 0, 2);
        public static MoveCommandBytes Backward = new MoveCommandBytes(0, -50, 0, 0, 2);
        public static MoveCommandBytes Right = new MoveCommandBytes(50, 0, 0, 0, 2);
        public static MoveCommandBytes Left = new MoveCommandBytes(-50, 0, 0, 0, 2);
        public static MoveCommandBytes TurnRight = new MoveCommandBytes(0, 0, 50, 0, 2);
        public static MoveCommandBytes TurnLeft = new MoveCommandBytes(0, 0, -50, 0, 2);
        public static MoveCommandBytes Hover = new MoveCommandBytes(0, 0, 0, 0, 1);

        public static WheelCommandBytes WheelOn = new WheelCommandBytes(true);
        public static WheelCommandBytes WheelOff = new WheelCommandBytes(false);


        public static FlipCommandBytes ForwardFlip = new FlipCommandBytes(Direction.Front);
        public static FlipCommandBytes BackwardsFlip = new FlipCommandBytes(Direction.Back);
        public static FlipCommandBytes RightFlip = new FlipCommandBytes(Direction.Right);
        public static FlipCommandBytes LeftFlip = new FlipCommandBytes(Direction.Left);
    }

    public class Command
    {
        private CommandBytes commandBytes;
        private Channel channel;

        public byte[] DataBytes => commandBytes.GetCommandBytes();

        public Command(Channel channel, CommandBytes commandBytes)
        {
            this.commandBytes = commandBytes;
            this.channel = channel;
        }

        public async Task Execute()
        {
            await channel.SendData(commandBytes);
        }
    }

    public class TakeOff : Command
    {
        public TakeOff(Channel channel): base(channel, new CommandBytes(0x04, 0x00, 0x01, 0x00))
        {
            
        }
    }

    public class Landing : Command
    {
        public Landing(Channel channel): base(channel, new CommandBytes(0x04, 0x00, 0x03, 0x00))
        {

        }
    }


    public class FlipCommandBytes : CommandBytes
    {
        public Direction direction { get; }

        public FlipCommandBytes(Direction direction) : base(2,4,0,0)
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

    public class WheelCommandBytes : CommandBytes
    {
        public byte WheelOn { get; set; }

        public WheelCommandBytes(bool wheelOn) : base(2, 1, 2, 0, 2)
        {
            WheelOn = BitConverter.GetBytes(wheelOn)[0];
        }

        public new byte[] GetCommandBytes()
        {
            return new[] { FirstValue, CommandCounter, ProjectId, ClassId, CommandId, ArgumentId, WheelOn };
        }
    }


    public class MoveCommandBytes: CommandBytes
    {
        public byte Roll { get; }
        public byte Pitch { get; }
        public byte Yaw { get; }
        public byte Up { get; }
        public int Steps { get; set; }

        public MoveCommandBytes(short roll, short pitch, short yaw, short up, int steps) : base(2, 0, 2, 0)
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

    public class CommandBytes
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
        public CommandBytes(byte firstValue, byte classId, byte commandId, byte argumentId, byte projectId = 0x02)
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
