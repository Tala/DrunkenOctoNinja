using System;

namespace FlyWithMe
{
    public static class ParrotUuids
    {
        /// <summary>
        /// "Controller to Device non-ack" aka write only
        /// Uses:
        /// A01, Stop, Movement, TakeOffAndLand, EmergencyStop, InitCount1To20ic, A1F
        /// </summary>
        public static Guid Service_A00 = new Guid("9a66fa00-0800-9191-11e4-012d1540cb8e");

        /// <summary>
        /// "Controller to Device ack" aka notify
        /// Uses:
        /// B01, B0Ebc_Bd, Battery_B0Fbf_C0, B1Be3_E4, B1Ce6_E7, B1F
        /// </summary>
        public static Guid Service_B00 = new Guid("9a66fb00-0800-9191-11e4-012d1540cb8e");

        /// <summary>
        /// "Controller to Device emergency" aka WriteWithoutResponse | Write
        /// Uses:
        /// FC1
        /// </summary>
        public static Guid Service_C00 = new Guid("9a66fc00-0800-9191-11e4-012d1540cb8e");

        // Unused? I got no connection
        public static Guid Service_E00 = new Guid("9a66fe00-0800-9191-11e4-012d1540cb8e");

        /// <summary>
        /// "Device to Controller event" aka read | WriteWithoutRepons | Write | Notify
        /// Uses:
        /// D22, D23, D24
        /// </summary>
        public static Guid Service_D21 = new Guid("9a66fd21-0800-9191-11e4-012d1540cb8e");

        /// <summary>
        /// "Device to Controller navdata" aka read | WriteWithoutRepons | Write | Notify
        /// Uses:
        /// D52, D53, D54
        /// </summary>
        public static Guid Service_D51 = new Guid("9a66fd51-0800-9191-11e4-012d1540cb8e");


        public static Guid Characteristic_A01 = new Guid("9a66fa01-0800-9191-11e4-012d1540cb8e"); 
        public static Guid Characteristic_A02 = new Guid("9a66fa02-0800-9191-11e4-012d1540cb8e"); 
        public static Guid Characteristic_A0A_Movement = new Guid("9a66fa0a-0800-9191-11e4-012d1540cb8e"); 
        /// <summary>
        /// Sendet Commands: Flips, TakeOff, Landing, FlatTrimm
        /// </summary>
        public static Guid Characteristic_A0B_SimpleCommands = new Guid("9a66fa0b-0800-9191-11e4-012d1540cb8e");  // DateTime?
        public static Guid Characteristic_A0C_EmergencyStop	= new Guid("9a66fa0c-0800-9191-11e4-012d1540cb8e"); 
        public static Guid Characteristic_A1E_InitCount1To20 = new Guid("9a66fa1e-0800-9191-11e4-012d1540cb8e");
        public static Guid Characteristic_A1F = new Guid("9a66fa1f-0800-9191-11e4-012d1540cb8e");
        public static Guid Characteristic_B01 = new Guid("9a66fb01-0800-9191-11e4-012d1540cb8e"); 
        public static Guid Characteristic_B0E_DroneState = new Guid("9a66fb0e-0800-9191-11e4-012d1540cb8e");
        public static Guid Characteristic_B0F_Battery = new Guid("9a66fb0f-0800-9191-11e4-012d1540cb8e");
        public static Guid Characteristic_B1B = new Guid("9a66fb1b-0800-9191-11e4-012d1540cb8e");
        public static Guid Characteristic_B1C = new Guid("9a66fb1c-0800-9191-11e4-012d1540cb8e");
        public static Guid Characteristic_B1F = new Guid("9a66fb1f-0800-9191-11e4-012d1540cb8e");
        public static Guid Characteristic_C1 = new Guid("9a66ffc1-0800-9191-11e4-012d1540cb8e"); 
        public static Guid Characteristic_D22 = new Guid("9a66fd22-0800-9191-11e4-012d1540cb8e");
        public static Guid Characteristic_D23 = new Guid("9a66fd23-0800-9191-11e4-012d1540cb8e");
        public static Guid Characteristic_D24 = new Guid("9a66fd24-0800-9191-11e4-012d1540cb8e");
        public static Guid Characteristic_D52 = new Guid("9a66fd52-0800-9191-11e4-012d1540cb8e");
        public static Guid Characteristic_D53 = new Guid("9a66fd53-0800-9191-11e4-012d1540cb8e");
        public static Guid Characteristic_D54 = new Guid("9a66fd54-0800-9191-11e4-012d1540cb8e");
    }

    public static class Services
    {
        public static Guid Command
        {
            get
            {   // [JH,TO] This is the id of the channel for commands
                // I dont get ACKs therefore I can only send instructions 
                // (and then not give a shit, pardon, FUCK)
                return ParrotUuids.Service_A00;
            }
        }
    }

    public static class Characteristics
    {
        public static Guid SimpleCommands {
            get { return ParrotUuids.Characteristic_A0B_SimpleCommands; }
        }
}
}
