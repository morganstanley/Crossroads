## Installation

To install crossroads as a global dotnet tool on the linux environment:

```sh
dotnet tool install -g MorganStanley.Crossroads
```
To get the list of available commands:

```sh
crossroads --help
```

## How to use

### Package Python

```sh
 dotnet Crossroads.dll package --name newnotepad --command "python3"
```

### Package Python and add a script file from include

```sh
dotnet Crossroads.dll package --name newhello  --command "python3" --args "script/crosspy.py" --location "./output" --include "../script" 
```

### Inspect a package

```sh
dotnet Crossroads.dll inspect --package "./newhello"
```

### Show help

```sh
Crossroads.dll --help
```

### Execute generated app

```sh
./<appname>
```

## How to Develop

### Build the solution

```sh
dotnet build
```

### Test and test with coverage

```sh
dotnet test
```

```sh
dotnet test -maxcpucount:1 -p:CollectCoverage=true -p:CoverletOutput="../TestResults/" -p:MergeWith="../TestResults/coverage.json"
```

### Deploy

```sh
dotnet build -c:release
dotnet publish ./src/Crossroads/Crossroads.csproj -c:release --no-build
```
