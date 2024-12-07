$currentDir = (Get-Location).Path

# Run Consumers of project
wt new-tab --title "Consumer1"  powershell.exe -Command "
cd \"$currentDir\CompetingConsumers.Consumer1\";
dotnet run" `;

wt new-tab --title "Consumer2"  powershell.exe -Command "
cd \"$currentDir\CompetingConsumers.Consumer2\";
dotnet run" `;

wt new-tab --title "Consumer3"  powershell.exe -Command "
cd \"$currentDir\CompetingConsumers.Consumer3\";
dotnet run" `;

Start-Sleep -Seconds 1

# Run the Producer project in a new tab
wt new-tab --title "Producer" powershell.exe -Command "
cd \"$currentDir\CompetingConsumers.Producer\";
dotnet run"
