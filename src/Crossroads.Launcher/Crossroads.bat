@ECHO OFF
ECHO Hellooo crossroads...!
cmd /c "dotnet build -r linux-x64 Crossroads.Launcher.csproj"
cmd /c "dotnet build -r win-x64 Crossroads.Launcher.csproj"
PAUSE