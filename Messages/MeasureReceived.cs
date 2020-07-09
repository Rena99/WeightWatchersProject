using NServiceBus;

namespace Messages
{
    public class MeasureReceived:IEvent
    {
        public int Id { get; set; }
        public int CardId { get; set; }
        public decimal Weight { get; set; }
    }
}
