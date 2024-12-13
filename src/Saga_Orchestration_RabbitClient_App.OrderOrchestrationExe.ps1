$currentDir = (Get-Location).Path+"\Saga"

wt new-tab  --title "Payment Consumer"   powershell.exe -Command "
cd \"$currentDir\Saga_Orchestration_RabbitClient_App.PaymentConsumer\";
dotnet run" `;

Start-Sleep -Seconds 3

wt new-tab  --title "Stock Consumer"   powershell.exe -Command "
cd \"$currentDir\Saga_Orchestration_RabbitClient_App.StockConsumer\";
dotnet run" `;

Start-Sleep -Seconds 3

wt new-tab  --title "Delivery Consumer"   powershell.exe -Command "
cd \"$currentDir\Saga_Orchestration_RabbitClient_App.DeliveryConsumer\";
dotnet run" `;

Start-Sleep -Seconds 3

wt new-tab  --title "Order Saga Orchestrator"   powershell.exe -Command "
cd \"$currentDir\Saga_Orchestration_RabbitClient_App.OrderOrchestrationProducer\";
dotnet run" `;
