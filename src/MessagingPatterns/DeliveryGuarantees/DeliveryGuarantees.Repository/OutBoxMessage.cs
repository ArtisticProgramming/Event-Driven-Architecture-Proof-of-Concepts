namespace DeliveryGuarantees.Repository
{
    public class OutBoxMessage
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Type { get; set; }
        public string Payload { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ProcessedAt { get; set; } = null;
    }
}