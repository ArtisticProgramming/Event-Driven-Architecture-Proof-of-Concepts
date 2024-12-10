$currentDir = (Get-Location).Path

wt new-tab  --title "Receiver"   powershell.exe -Command "
cd \"$currentDir\RpcApp.Response\";
dotnet run" `;


Start-Sleep -Seconds 3

wt new-tab  --title "Sender"   powershell.exe -Command "
cd \"$currentDir\RpcApp.Request\";
dotnet run" `;
