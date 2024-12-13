using Saga_Orchestration_RabbitClient_App.PaymentConsumer;
Console.WriteLine(@"

PaymentConsumer

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

var paymentConsumer = new PaymentConsumer();
paymentConsumer.Run();
paymentConsumer.Close();