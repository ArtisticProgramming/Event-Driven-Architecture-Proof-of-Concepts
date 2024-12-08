namespace RabbitMQExchanges.BuildingBlocks.Model
{
    public interface IEnvlopeWrapper
    {
        public string EnvlopeContent { get; set; }
        public bool IsEncrypted { get; set; }
    }
}