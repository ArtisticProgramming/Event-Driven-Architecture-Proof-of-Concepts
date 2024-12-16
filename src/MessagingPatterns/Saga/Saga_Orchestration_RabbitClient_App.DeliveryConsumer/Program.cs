using Saga_Orchestration_RabbitClient_App.DeliveryConsumer;

Console.WriteLine(@"

DeliveryConsumer

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
var paymentConsumer = new DeliveryConsumer();
paymentConsumer.Run();
paymentConsumer.Close();