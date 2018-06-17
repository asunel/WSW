namespace WSW.EventViewer
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Eventing.Reader;
    using System.Linq;

    public class EventViewer
    {
        private const string QueryFormat = "*[System/TimeCreated/@SystemTime >= '{0}']";
        private const string DateTimeFormatLogViewer = "{0}-{1}-{2}T{3}:{4}:{5}.000000000K";

        public EventRecord GetLog(string logName, string source, StandardEventLevel eventLevel)
        {
            if (!EventLog.SourceExists(source))
            {
                return new EventRecord { Source = string.Format(Constants.SourceDoesNotExist, source) };
            }

            var dt = DateTime.Now.AddHours(Constants.TimeIntervalForLogsToCheck);
            var formattedDateTime = string.Format(DateTimeFormatLogViewer,
                dt.Year,
                dt.Month,
                dt.Day,
                dt.Hour,
                dt.Minute,
                dt.Second);

            // Note: The query result does not work correctly. It shows some records less than FormattedDateTime as well.
            var query = string.Format(QueryFormat, formattedDateTime);
            var logQuery = new EventLogQuery(logName, PathType.LogName, query);

            var reader = new EventLogReader(logQuery);

            var recordList = new List<EventRecord>();
            System.Diagnostics.Eventing.Reader.EventRecord rec;

            while ((rec = reader.ReadEvent()) != null)
            {
                if (rec.Level == (byte)eventLevel && rec.ProviderName == source.Trim() && rec.TimeCreated > DateTime.Now.AddHours(Constants.TimeIntervalForLogsToCheck))
                {
                    recordList.Add(new EventRecord(rec));
                }
            }

            return recordList.OrderByDescending(r => r.TimeCreated).FirstOrDefault();
        }
    }
}
