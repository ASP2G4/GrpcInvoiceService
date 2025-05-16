using System.Text.Json.Serialization;

namespace Infrastructure.Messaging.ServiceBus.Models;

public class BookingCreatedMessage
{
    [JsonPropertyName("Id")]
    public int BookingId { get; set; }
    public int Tickets { get; set; }
    public string EventId { get; set; } = null!;
    public string UserId { get; set; } = null!;
}
