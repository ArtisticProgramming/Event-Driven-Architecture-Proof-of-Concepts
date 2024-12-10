$currentDir = (Get-Location).Path

wt new-tab  --title "Producer"   powershell.exe -Command "
cd \"$currentDir\PriorityQueueApp.Producer\";
dotnet run" `;


Start-Sleep -Seconds 4


wt new-tab  --title "Consumer1"   powershell.exe -Command "
cd \"$currentDir\PriorityQueueApp.Consumer1\";
dotnet run" `;