using Saga_Orchestration_RabbitClient_App.OrderOrchestrationProducer;

class Program 
{
    static void Main(string[] args)
    {
        try
        {
            Console.WriteLine(@"

OrderOrchestrationProducer

 $$$$$$\  $$$$$$\  $$$$$$\  $$$$$$\      
$$  __$$\$$  __$$\$$  __$$\$$  __$$\     
$$ /  \__$$ /  $$ $$ /  \__$$ /  $$ |    
\$$$$$$\ $$$$$$$$ $$ |$$$$\$$$$$$$$ |    
 \____$$\$$  __$$ $$ |\_$$ $$  __$$ |    
$$\   $$ $$ |  $$ $$ |  $$ $$ |  $$ |    
\$$$$$$  $$ |  $$ \$$$$$$  $$ |  $$ |    
 \______/\__|  \__|\______/\__|  \__|    
=====================================
");
            var orchestrator = new OrderSagaOrchestrator();
            orchestrator.Run();
            Console.ReadLine();
            orchestrator.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());   
        }
        finally
        {
        }
    }
}

