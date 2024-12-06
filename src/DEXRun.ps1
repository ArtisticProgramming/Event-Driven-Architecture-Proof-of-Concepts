# Run the first project in a new tab
wt new-tab powershell.exe -Command "
cd \"C:\Users\tree\source\repos\EDA-POC\src\DirectExchangeApp.Consumer1\";
dotnet run" `;


# Run the second project in a new tab
wt new-tab powershell.exe -Command "
cd \"C:\Users\tree\source\repos\EDA-POC\src\DirectExchangeApp.Producer\";
dotnet run"
