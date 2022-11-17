# Tutorial: Package a .Net console application using the Crossroads tool packager
This tutorial teaches you how to use the crossroads tool packaging manager. The <code>crossroads </code> tool generates a self-contained executable, making sure that others can install and run your package without having to install any dependencies. The <code>crossroads</code> tool is a NuGet package that is installed from the .NET CLI.

The console application that we'll package takes a message as input and displays the message along with lines of text that create the image of Morgan Stanley initials.

## Prerequisites
<li>.Net SDK  3.1.0 or later version</li>
This tutorial uses .NET SDK 3.1, but global tools are available starting in .NET Core SDK 2.1. Local tools are available starting in .NET Core SDK 3.0.
<li>Any .Net console applicaton</li>
<br>

## Usage
``` dotnet tool install --global MorganStanley.Crossroads --version 1.0.0 ```

The above command installs the crossroads as a global tool. This will enable you to run the tool from anywhere on your machine.

## Packaging our console app
Firstly, let's see the output of the console application 
<br>

<img alt="#" src =".\assets\tutorial_1.png">
<br>

Let's see the current folder structure of the .Net console application

<img alt="#" src=".\assets\folder_structure.png">

We now head straight to packaging our application and generating the single executable

## Command to package the botsay console application
``` crossroads package -n test-botsay --include "C:\repository\microsoft.botsay\bin\Debug\net6.0" -c ".\net6.0\microsoft.botsay.exe" -l "C:\OurBotsayOutput"```

Don't be confused just yet. We'll delve into the meaning of the command above.
<hr>
<code>crossroads</code> is the tool that we installed at the beginning of this tutorial and that is the base command we'll use whenever we want to use crossroads to package an application.

<code>package</code> is a sub-command on the crossroads tool. The package sub-command is used to create an executable package. Successfully packaging the console application should generate an executable with a given name, `test-botsay` in this case. We can see a list of commands and options on the crossroads tool by typing `crossroads -h` or just `crossroads`

`-n` This is an alias for the `--name` option on the crossroads tool. It allows us to specify a name for the executable that will be generated.

`--include` Include internal resource application to be packaged

`-c` An alias for the `--command` option on the crossroads tool. It allows us to specify the command to run the internal application, which is the `microsoft.botsay.exe` executable.

`-l` Another alias for the `--location` option. This option allows us to set the output file location of the package.

These are just a few of the options we have on the crossroads tool. There are others available for customizing your app with an icon, default args, etc.

## Help Pages for <code>crossroads -h</code> and <code>crossroads package -h</code> respectively <br>

<img alt ="#" src =".\assets\help.png"> <br>
The above shows the different commands and options on the `crossroads` tool.
<br>

<img alt ="#" src =".\assets\packagehelp.png">
This above image also shows the help page for the <code>package</code> sub-command of the crossroads tool. We can actually do the same for the other sub-commands on the crossroads tool. <br>
<br>

# Result

After successfully packaging an application, we get success feedback, otherwise the necessary error message.

<img alt ="#" src = ".\assets\package-success.png"> <br>


Our custom location which has the packaged application <br>
<img alt = "#" src =".\assets\output.png"> <br>


## Launching the application

Having packaged our console app, let's try running the created executable by using the command below:

```.\test-botsay.exe```  <br>

we navigate to the output directory and run the above command in the terminal. Below is the expected output: <br>

<img alt = "#" src = ".\assets\test-botsay.png">
<br>
Voila! There we have it! Same behavior from the original console application.
<br><br>


## Package Inspection
To see the metadata and or additional information in our packaged application, run the command below:

```crossroads inspect --package "test-botsay.exe"```
<br><br>
And the corresponding output below: <br><br>
<img alt = "#" src = ".\assets\botsay-inspect.png">
