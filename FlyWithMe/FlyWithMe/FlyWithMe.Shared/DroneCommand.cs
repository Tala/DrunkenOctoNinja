using System;

namespace FlyWithMe
{
    public enum CommandId
    {
        Ack,
        NonAck,
        NavData,
        Event
    }

    public class DroneCommand
    {
        public CommandId Id { get; private set; }
        public object DataType { get; private set; }
        public int SendingWaitTimeMs { get; private set; }
        public int AckTimeoutMs { get; private set; }
        public int NumberOfRetry { get; private set; }
        public int NumberOfCell { get; private set; }
        public int DataCopyMaxSize { get; private set; }
        public bool IsOverwriting { get; private set; }

        public DroneCommand(CommandId id, object dataType, int sendingWaitTimeMs, int ackTimeoutMs, int numberOfRetry, int numberOfCell, int dataCopyMaxSize, bool isOverwriting)
        {
            Id = id;
            DataType = dataType;
            SendingWaitTimeMs = sendingWaitTimeMs;
            AckTimeoutMs = ackTimeoutMs;
            NumberOfRetry = numberOfRetry;
            NumberOfCell = numberOfCell;
            DataCopyMaxSize = dataCopyMaxSize;
            IsOverwriting = isOverwriting;
        }
    }

    public static class DroneCommands
    {
        /// .ID = ROLLINGSPIDER_CONTROLLER_TO_DEVICE_EMERGENCY_ID,
        /// .dataType = ARNETWORKAL_FRAME_TYPE_DATA_WITH_ACK,
        /// .sendingWaitTimeMs = 1,
        /// .ackTimeoutMs = 100,
        /// .numberOfRetry = ARNETWORK_IOBUFFERPARAM_INFINITE_NUMBER,
        /// .numberOfCell = 1,
        /// .dataCopyMaxSize = ARNETWORK_IOBUFFERPARAM_DATACOPYMAXSIZE_USE_MAX,
        /// .isOverwriting = 0, 
        public static DroneCommand Get_EmergencyCommand()
        {
            return new DroneCommand(CommandId.Ack, 4, 1, 100, Int32.MaxValue, 1, Int32.MaxValue,false );
        }

        /*.ID = ROLLINGSPIDER_CONTROLLER_TO_DEVICE_ACK_ID,
        .dataType = ARNETWORKAL_FRAME_TYPE_DATA_WITH_ACK,
        .sendingWaitTimeMs = 20,
        .ackTimeoutMs = 500,
        .numberOfRetry = 3,
        .numberOfCell = 20,
        .dataCopyMaxSize = ARNETWORK_IOBUFFERPARAM_DATACOPYMAXSIZE_USE_MAX,
        .isOverwriting = 0,*/
        public static DroneCommand Get_AckCommand()
        {
            return new DroneCommand(CommandId.Ack, 4, 20, 500, 3, 20, Int32.MaxValue, false );
        }


        /*.ID = ROLLINGSPIDER_CONTROLLER_TO_DEVICE_NONACK_ID,
        .dataType = ARNETWORKAL_FRAME_TYPE_DATA,
        .sendingWaitTimeMs = 20,
        .ackTimeoutMs = ARNETWORK_IOBUFFERPARAM_INFINITE_NUMBER,
        .numberOfRetry = ARNETWORK_IOBUFFERPARAM_INFINITE_NUMBER,
        .numberOfCell = 1,
        .dataCopyMaxSize = ARNETWORK_IOBUFFERPARAM_DATACOPYMAXSIZE_USE_MAX,
        .isOverwriting = 1,*/
        public static DroneCommand Get_NonAckCommand()
        {
            return new DroneCommand(CommandId.NonAck, 2, 20, Int32.MaxValue, Int32.MaxValue, 1, Int32.MaxValue, true);
        }
    }
}
