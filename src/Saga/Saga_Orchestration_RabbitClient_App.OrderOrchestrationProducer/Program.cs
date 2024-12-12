using Saga_Orchestration_RabbitClient_App.OrderOrchestrationProducer;

class Program : BaseRabbitMq
{
    static void Main(string[] args)
    {
        try
        {
            Start();

            OrderSagaOrchestration sagaOrchestration = new OrderSagaOrchestration();
            sagaOrchestration.Execute();

            WaitForEnter();
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
        finally
        {
            Stop();
        }
    }
}

