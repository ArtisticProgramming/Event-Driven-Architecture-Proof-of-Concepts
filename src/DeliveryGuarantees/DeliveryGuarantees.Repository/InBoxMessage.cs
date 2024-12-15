namespace DeliveryGuarantees.Repository
{
    public class InBoxMessage
    {
        public Guid Id { get; set; }
        public int ObjectId { get; set; }
        public string Type { get; set; }
        public string Payload { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ProcessedAt { get; set; } = null;
    }
}