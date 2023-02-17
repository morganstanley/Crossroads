# Crossroads (Packaging tool)

![Lifecycle Active](https://badgen.net/badge/Lifecycle/Active/green)
[![CI](https://github.com/morganstanley/Crossroads/actions/workflows/continuous-integration.yml/badge.svg)](https://github.com/morganstanley/Crossroads/actions/workflows/continuous-integration.yml)

## Description

Crossroads is a dotnet core commandline tool packager for developers. This is a generic solution to host any application within Crossroads package executable and further launches application's executable. Developers will specify arguments such as name, icon, version etc for branding during the package generation. The specified argument name will be used to rebrand the internal application.

### Why Use Crossroads

Crossroads allows you to:

- create an executable package.
- customise your package with a name, icon, version and other attributes
- run applications through crossroads generated package

## Installation

To install crossroads as a global dotnet tool:

```powershell
dotnet tool install -g MorganStanley.Crossroads
```
To get the list of available commands:

```powershell
crossroads --help
```

## How to use

### Package Notepad

```powershell
crossroads package --name newnotepad --command "notepad"
```

### Package Notepad and open a text file from include

```powershell
crossroads package --name newhello --command notepad --args .\notepadtxt\abc.txt --location .\output --icon .\testicon.ico --version "2.2.2" --include ".\notepadtxt"
```

### Package an application from include directory

```powershell
crossroads package --name newhello --command ".\helloworld\helloworld.exe" --location .\output --icon .\testcion.ico --version "3.0.1" --include ".\helloworld"
```

### Inspect a package

```powershell
crossroads inspect --package ".\testapp.exe"
```

### Show help

```powershell
crossroads --help
```

### Execute generated app

```powershell
<appname>.exe
```

### Execute generated with override arguments

```powershell
<appname>.exe --args "new args"
```

## How to Develop

### Build the solution

```powershell
dotnet build
```

### Test and test with coverage

```powershell
dotnet test
```

```powershell
dotnet test -maxcpucount:1 -p:CollectCoverage=true -p:CoverletOutput="..\TestResults\" -p:MergeWith="..\TestResults\coverage.json"
```

### Deploy

```powershell
dotnet build -c:release
dotnet publish .\src\Crossroads\Crossroads.csproj -c:release --no-build
```
### Crossroads on Linux
For information about how to use our application on the linux environment kindly <a href="https://github.com/morganstanley/Crossroads/blob/main/doc/Linux.md" target="_blank">click here</a>.