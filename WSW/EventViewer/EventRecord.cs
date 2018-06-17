namespace WSW.EventViewer
{
    using System;
    using static WSW.Constants;

    public class EventRecord
    {
        public EventRecord()
        {
        }

        public EventRecord(System.Diagnostics.Eventing.Reader.EventRecord rec)
        {
            TimeCreated = rec.TimeCreated;
            Source = rec.ProviderName;
            EventId = rec.Id;
            Message = rec.FormatDescription()?.Replace(Environment.NewLine, HtmlBrTag);
            User = rec.UserId == null ? UserNa : rec.UserId.ToString();
            Level = rec.LevelDisplayName;
            Computer = rec.MachineName;
        }

        public DateTime? TimeCreated { get; set; }

        public string Source { get; set; }

        public long EventId { get; set; }

        public string Message { get; set; }

        // Warning! Don't delete this zero-referenced property. It is used using reflection
        public string MessagePreview => Message == null ? string.Empty : Message.Split("\n.".ToCharArray())[0];

        public string Level { get; set; }

        public string Computer { get; set; }

        public string User { get; set; }
    }
}
