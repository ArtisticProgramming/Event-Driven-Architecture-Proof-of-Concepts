$currentDir = (Get-Location).Path

# Run the Consumer1 project in a new tab
wt new-tab --title "Consumer1"   powershell.exe -Command "
cd \"$currentDir\DeadLetterNegativelyAcknowledgedApp.Consumer1\";
dotnet run" `;

Start-Sleep -Seconds 2

# Run the Producer project in a new tab
wt new-tab --title "Producer"  powershell.exe -Command "
cd \"$currentDir\DeadLetterNegativelyAcknowledgedApp.Producer\";
dotnet run"
