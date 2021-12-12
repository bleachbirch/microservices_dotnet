namespace EventFeed
{
    public class Event
    {
        public string EventName { get; set; }
        public DateTimeOffset CreationDateTime { get; set; } = DateTimeOffset.UtcNow;
        public long SequenceNumber { get; set; }
        public object Content { get; set; }
    }
}
