namespace DeliveryGuarantees.Repository
{
    public class InBoxMessage
    {
        public InBoxMessage()
        {
            
        }
        public Guid Id { get; set; } = Guid.NewGuid();  
        public required string MessageType  { get; set; }
        public required string Payload { get; set; }
        public string? CorrelationId { get; set; }
        public string? Header { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ProcessedAt { get; set; } = null;
    }
}