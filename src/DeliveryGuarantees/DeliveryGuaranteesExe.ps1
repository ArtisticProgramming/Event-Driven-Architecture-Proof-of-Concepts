$currentDir = (Get-Location).Path

wt new-tab  --title "InBox - Consumer"   powershell.exe -Command "
cd \"$currentDir\InBoxApp.Consumer\";
dotnet run" `;

Start-Sleep -Seconds 3

wt new-tab  --title "OutBox - Producer"   powershell.exe -Command "
cd \"$currentDir\OutBoxApp.Producer\";
dotnet run" `;
