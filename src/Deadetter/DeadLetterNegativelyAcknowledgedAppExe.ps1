$currentDir = (Get-Location).Path

wt new-tab --title "Consumer1"   powershell.exe -Command "
cd \"$currentDir\DeadLetterNegativelyAcknowledgedApp.Consumer1\";
dotnet run" `;

Start-Sleep -Seconds 2

wt new-tab --title "Producer"  powershell.exe -Command "
cd \"$currentDir\DeadLetterNegativelyAcknowledgedApp.Producer\";
dotnet run"
