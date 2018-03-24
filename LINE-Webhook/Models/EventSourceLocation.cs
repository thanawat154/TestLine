using Microsoft.WindowsAzure.Storage.Table;

namespace LINE_Webhook.Models
{
    public class EventSourceLocation : EventSourceState
    {
        public string Location { get; set; }

        public EventSourceLocation() { }
    }
}