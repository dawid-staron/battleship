Run application from command line

Prerequisites: .NET 6.0 SDK https://dotnet.microsoft.com/en-us/download/dotnet/6.0 must be installed on executing machine

Steps
1.  Download repository content or clone repository
2.  Navigate into folder where solution file Battleship.sln is placed
3.  From command line execute 'dotnet run --project ./src/Battleship.UI -c Release' to start playing
4*. To execute test suite run from command line 'dotnet test Battleship.sln -c Release'