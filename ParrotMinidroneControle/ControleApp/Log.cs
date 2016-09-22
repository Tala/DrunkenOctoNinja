using System;
using System.Globalization;
using System.Threading.Tasks;
using Windows.Foundation.Diagnostics;

namespace ParrotMiniDroneControle
{
    public class Log
    {
        public static Log Instance { get; } = new Log();
        public LoggingChannel Channel;
        public FileLoggingSession Session;
        public EventHandler<CustomEventArgs> MessageLogged;

        public Log()
        {
            Channel = new LoggingChannel("DroneLogChannel", new LoggingChannelOptions());
            Session = new FileLoggingSession("Drone Session");
            
            Session.AddLoggingChannel(Channel, LoggingLevel.Information);
        }

        private string GetTimeStamp()
        {
            var now = DateTime.Now;
            return string.Format(CultureInfo.InvariantCulture,
                "{0:D2}{1:D2}{2:D2}-{3:D2}{4:D2}{5:D2}{6:D3}",
                now.Year - 2000,
                now.Month,
                now.Day,
                now.Hour,
                now.Minute,
                now.Second,
                now.Millisecond);
        }


        public void LogMessage(string message)
        {
            Channel.LogMessage(GetTimeStamp() + ": " + message);
            MessageLogged?.Invoke(this, new CustomEventArgs(message));
        }
        
        public void LogCommandFromDrone(string message, byte[] command)
        {
            LogMessage("From Drone:" + message + command.ToString());
        }

        public void LogCommandToDrone(string message, byte[] command)
        {
            LogMessage("To Drone:" + message + command.ToString());
        }

        public async Task CloseSession()
        {
            var storageFile = await Session.CloseAndSaveToFileAsync();
        }
    }
}