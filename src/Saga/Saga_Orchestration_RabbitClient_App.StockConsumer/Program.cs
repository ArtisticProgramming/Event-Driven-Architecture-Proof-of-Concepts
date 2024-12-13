using Saga_Orchestration_RabbitClient_App.StockConsumer;
Console.WriteLine(@"

StockConsumer

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
var paymentConsumer = new StockConsumer();
paymentConsumer.Run();
paymentConsumer.Close();