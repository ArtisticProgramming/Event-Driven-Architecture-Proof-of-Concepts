using DeliveryGuarantees.Repository;
using InBoxApp.Producer.Services;
using Newtonsoft.Json;
using RabbitMQExchanges.BuildingBlocks.Model;

class Program : EDA_Utilities.BaseRabbitMq
{
    static void Main(string[] args)
    {
        try
        {
            using (var db = new AppDbContext())
            {
                var outBoxService = new OutboxService(db);

                LogInfo(@"
  ______               __      _______                      
 /      \             /  |    /       \                     
/$$$$$$  | __    __  _$$ |_   $$$$$$$  |  ______   __    __ 
$$ |  $$ |/  |  /  |/ $$   |  $$ |__$$ | /      \ /  \  /  |
$$ |  $$ |$$ |  $$ |$$$$$$/   $$    $$< /$$$$$$  |$$  \/$$/ 
$$ |  $$ |$$ |  $$ |  $$ | __ $$$$$$$  |$$ |  $$ | $$  $$<  
$$ \__$$ |$$ \__$$ |  $$ |/  |$$ |__$$ |$$ \__$$ | /$$$$  \ 
$$    $$/ $$    $$/   $$  $$/ $$    $$/ $$    $$/ /$$/ $$  |
 $$$$$$/   $$$$$$/     $$$$/  $$$$$$$/   $$$$$$/  $$/   $$/ 
                                                            
");

                while (true)
                {
                    try
                    {
                        RigisterUser();

                        UserRegisteredEvent userRegisteredEvent =
                          new UserRegisteredEvent("Mostafa", "Jafari", new Random().Next(1, 120));


                        outBoxService.AddMessage(type: nameof(UserRegisteredEvent).ToString(),
                           payload: JsonConvert.SerializeObject(userRegisteredEvent));

                        Thread.Sleep(3000);

                        //If RabbitMq is not ready. Application proccessing messages
                        //and save in Db unitle Rabbitmq get ready.
                        //For test you can stop rabbitmq at the begining and then run it.
                        var OutboxProcessor = new OutboxProcessor(db);
                        LogInfo("OutboxProcessor running...");
                        OutboxProcessor.Process();
                        Console.WriteLine("------------------------------------------");
                    }
                    catch (Exception ex)
                    {
                        LogError(ex.ToString());
                    }
                }
            }
        }
        catch (Exception ex)
        {
            LogError(ex.ToString());
        }
        finally
        {
        }
    }

    private static void RigisterUser()
    {
        //Proccess and Save User data in DB
    }
}