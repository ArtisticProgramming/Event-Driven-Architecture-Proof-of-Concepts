using RpcApp.Request;

var rpcClient = new RpcRequest();

while (true)
{
    Console.WriteLine("Ask somthing:");
    var input = Console.ReadLine();

    rpcClient.Call(input);

    Console.WriteLine($"{DateTime.Now.ToLongTimeString()} | request sent.");

    Console.ReadLine();
}


Console.WriteLine("Press enter to exit.");

rpcClient.Close();