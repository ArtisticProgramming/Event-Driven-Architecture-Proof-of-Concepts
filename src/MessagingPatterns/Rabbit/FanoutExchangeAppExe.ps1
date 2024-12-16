$currentDir = (Get-Location).Path

wt new-tab  --title "Consumer1"   powershell.exe -Command "
cd \"$currentDir\FanoutExchangeApp.Consumer1\";
dotnet run" `;

Start-Sleep -Seconds 3

wt new-tab  --title "Consumer2"   powershell.exe -Command "
cd \"$currentDir\FanoutExchangeApp.Consumer2\";
dotnet run" `;

Start-Sleep -Seconds 3

wt new-tab  --title "Producer"   powershell.exe -Command "
cd \"$currentDir\FanoutExchangeApp.Producer\";
dotnet run" `;
