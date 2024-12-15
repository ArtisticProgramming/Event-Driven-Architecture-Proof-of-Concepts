$currentDir = (Get-Location).Path

wt new-tab --title "Consumer1"  powershell.exe -Command "
cd \"$currentDir\DoubleWritingApp.Consumer1\";
dotnet run" `;

Start-Sleep -Seconds 3

wt new-tab  --title "Producer"   powershell.exe -Command "
cd \"$currentDir\DoubleWritingApp.Producer\";
dotnet run"
