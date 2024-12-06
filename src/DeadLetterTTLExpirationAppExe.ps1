$currentDir = (Get-Location).Path

# Run the Consumer1 project in a new tab
wt new-tab --title "Consumer1"  powershell.exe -Command "
cd \"$currentDir\DeadLetterTTLExpirationApp.Consumer1\";
dotnet run" `;

Start-Sleep -Seconds 1

# Run the Producer project in a new tab
wt new-tab --title "Producer" powershell.exe -Command "
cd \"$currentDir\DeadLetterTTLExpirationApp.Producer\";
dotnet run"
