$currentDir = (Get-Location).Path

# Run the Consumer1 project in a new tab
wt new-tab powershell.exe -Command "
cd \"$currentDir\DoubleWritingApp.Consumer1\";
dotnet run" `;

Start-Sleep -Seconds 2

# Run the Producer project in a new tab
wt new-tab powershell.exe -Command "
cd \"$currentDir\DoubleWritingApp.Producer\";
dotnet run"
