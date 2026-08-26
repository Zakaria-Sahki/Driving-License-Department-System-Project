using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;

namespace DVLD_DataAccessLayer
{
    public class clsEventLogger {

        static public string SourceName = "DVLD.App";

        public static void LogInformation(Exception Ex, string Message) {


            LogEventToEventViewer(Ex, Message, EventLogEntryType.Information);
        }
        public static void LogWarning(Exception Ex, string Message) {

            LogEventToEventViewer(Ex, Message, EventLogEntryType.Warning);
        }
        public static void LogError(Exception Ex, string Message) {

            LogEventToEventViewer(Ex, Message, EventLogEntryType.Error);
        }
        
        
        
        private static void Initialize() {

            // Create the event source if it does not exist
            if (!EventLog.SourceExists(SourceName))
            {
                EventLog.CreateEventSource(SourceName, "Application");
            }
        }
        private static void EventWrite(string Message, EventLogEntryType EventLogType)
        {
            try {

                EventLog.WriteEntry(SourceName, Message, EventLogType);
            }
            catch (Exception ex) {

                Console.WriteLine($"An Error was occured while logging the event: {ex.Message}");
            }
        }
        private static void LogEventToEventViewer(Exception Ex, string Message, EventLogEntryType EventLogType) {


            Initialize();

            // Log an information event
            
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("===================================================");
            sb.AppendLine($"Date: {DateTime.Now.ToShortTimeString()}");
            sb.AppendLine($"User Message: {Message}");
            sb.AppendLine($"Exception Message: {Ex.Message}");
            sb.AppendLine("===================================================");

            string TotalMessage = sb.ToString();
            EventWrite(TotalMessage, EventLogType);
        }
    }
}
