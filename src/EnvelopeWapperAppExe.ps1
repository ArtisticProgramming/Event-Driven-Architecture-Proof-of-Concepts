$currentDir = (Get-Location).Path

wt new-tab --title "Consumer1"  powershell.exe -Command "
cd \"$currentDir\EnvelopeWrapper.Consumer1\";
dotnet run" `;

Start-Sleep -Seconds 3

wt new-tab  --title "Producer"   powershell.exe -Command "
cd \"$currentDir\EnvelopeWrapper.Producer\";
dotnet run"
