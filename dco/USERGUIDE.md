# Tutorial - Package a java console application using the Crossroads tool packager

This tutorial teaches you how to use the crossroads tool packaging manager even though built with the .net framework to package a simple java console application. The <code>crossroads </code> tool generates a self-contained executable, making sure that others can install and run your package without having to install any dependencies. The <code>crossroads</code> tool is a NuGet package that is installed from the .NET CLI.

The console application that we will package is a simple application that displays the  "Hello world!" message.

## Pre-requisites
<li>Java 17 or later version</li>
<li>Any java console applicaton</li>
<br>
Note:
You don't need to install .Net in order to use the crossroads package. 

## Usage
``` dotnet tool install --global MorganStanley.Crossroads --version 1.0.0 ```

The above command installs the crossroads as a global tool. This will enable you to run the tool from anywhere on your machine.

## Testing our console application
Firstly, let's see the output of the console application:
<br>
<img alt="#" src =".\assets\java_output.png">
<br>

let's see the current folder structure of the JavaCrossroads console application

<img alt="#" src=".\assets\folder_structure.png">

## Packaging our console application
We now head straight to packaging our application and generating the single executable

## Command to package the javacroassroads console application
``` crossroads package --name javacrossroads-test --command "openjdk-18.0.2\bin\java.exe" --location "C:\OurJavaCrossroadsOutput"  --include "C:\Users\User\.jdks\openjdk-18.0.2" --include "C:\Users\User\Documents\JavaCrossroads\src"  --args "src\Main.java" ```

<img alt="#" src=".\assets\package_success.png">

now let's take a look into what each command represents:

<hr>
<code>crossroads</code> is the tool that we installed globally at the beginning of this tutorial and that is the base command we'll use whenever we want to use crossroads to package an application.

<code>package</code> is a sub-command on the crossroads tool. The package sub-command is used to create an executable package. Upon successfully packaging the console application this should generate an executable with a given name, `javacrossroads-test` in our case.
We can see a list of commands and options on the crossroads tool by typing `crossroads -h` or just `crossroads`
`--name`: It allows us to specify a name for the executable that will be generated.
`--include`: Include internal resource application to be packaged
`--command`:This command can have the alias `-c ` option on the crossroads tool. It allows us to specify the command to run the internal application, which in our case is the `java.exe` executable.
`--location`: This option allows us to set the output file location of the package.

These are just a few of the options we have on the crossroads tool. There are others available for customizing your app with an icon, default args, etc.

## Help Pages for <code>crossroads -h</code> and <code>crossroads package -h</code> respectively <br>

<img alt ="#" src =".\assets\help.png"> <br>

The above shows the different commands and options on the `crossroads` tool.
<br>
<img alt ="#" src =".\assets\help.png">
This above image also shows the help page for the <code>package</code> sub-command of the crossroads tool. We can actually do the same for the other sub-commands on the crossroads tool. <br>
<br>
# Result 

After successfully packaging an application, we get success feedback, otherwise the necessary error message.

<img alt ="#" src = ".\assets\package_success.png"> <br>
Our custom location which has the packaged application <br>
<img alt = "#" src =".\assets\package_output.png"> <br>

## Launching the application
Having packaged our java console app , let's try running the created executable by using the command below:
```.\javacrossroads-test.exe```  <br>
we navigate to the output directory and run the above command in the terminal. Below is the expected output: <br>
<img alt = "#" src = ".\assets\test-javacrossroads.png">
<br>
Voila! There we have it! Same behavior from the original console application.
<br><br>
## Package Inspection
To see the metadata and or additional information in our packaged application, run the command below:
```crossroads inspect --package "javacrossroads-test.exe"```
<br><br>
And the corresponding output below: <br><br>
<img alt = "#" src = ".\assets\javacrossroads-inspect.png">



