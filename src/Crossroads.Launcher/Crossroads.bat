@ECHO OFF
cmd /c "dotnet build -r linux-x64 Crossroads.Launcher.csproj"
cmd /c "dotnet build -r win-x64 Crossroads.Launcher.csproj"
cmd /c "dotnet build ../Crossroads/Crossroads.csproj"
EXIT