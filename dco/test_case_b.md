# Tutorial - Package a java console application using the Crossroads tool packager

This tutorial teaches you how to use the crossroads tool packager which is built with the .net framework to package another application built with a different framework eg. java. For the purpose of this tutorial we will be using a simple java application called javacrossroads that displays a "Hello world!" message. The <code>crossroads </code> tool will generate a self-contained executable, making sure that others can install and run our package without having to install any dependencies. The <code>crossroads</code> tool is a NuGet package that is installed from the .NET CLI.

## Pre-requisites
<li>.Net SDK  3.1.0 or later version</li>
This tutorial uses .NET SDK 3.1, but global tools are available starting in .NET Core SDK 2.1. Local tools are available starting in NET Core SDK 3.0.
<li>Java 18 or later version</li>
<li>Any java console application</li>
<br>

## Usage
``` dotnet tool install --global MorganStanley.Crossroads --version 1.0.0 ```

The above command installs the crossroads as a global tool. This will enable you to run the tool from anywhere on your machine.

## Testing our console application

First,let's see the output of the javacrossroads console application:

<br>
<img alt="#" src =".\assets\java_output.png">
<br>

let's see the current folder structure of the JavaCrossroads console application

<img alt="#" src=".\assets\javafolder_structure.png">

## Packaging our console application
We now head straight to packaging our application and generating the single executable

## Command to package the JavaCrossroads console application

```crossroads package --name javacrossroads-test --command "openjdk-18.0.2\bin\java.exe" --location "C:\OurJavaCrossroadsOutput"  --include "C:\Users\User\.jdks\openjdk-18.0.2" --include "C:\JavaCrossroads\src"  --args "src\Main.java"```

<img alt="#" src=".\assets\package_success.png">

Now let's take a look into what each command represents:

<hr>
<code>crossroads</code> is the tool that we installed globally at the beginning of this tutorial and that is the base command we'll use whenever we want to use crossroads to package an application.

<code>package</code> is a sub-command on the crossroads tool. The package sub-command is used to create an executable package. Upon successfully packaging the console application this should generate an executable with a given name, `javacrossroads-test` in our case.
We can see a list of commands and options on the crossroads tool by typing `crossroads -h` or just `crossroads`.

`--name or -n`:this command allows us to specify a name for the executable that will be generated.

`--include`: Include internal resource application to be packaged.
The --include command can be used multiple times especially in scenarios where the dependencies needed to run the framework plus the project files are found in two separate file locations.
This can be seen in this instance where we need all the dependencies in the jdk folder inorder to run our application as well as the path to the main class.

`--command`:This command can have the alias `-c ` option on the crossroads tool. It allows us to specify the command to run the internal application, which in our case is the `java.exe` executable.

`--location`: This option allows us to set the output file location of the package.

These are just a few of the options we have on the crossroads tool. There are others available for customizing your app with an icon, default args, etc.

## Help Pages for <code>crossroads -h</code> and <code>crossroads package -h</code> respectively <br>

<img alt ="#" src =".\assets\crossroads_help_result.png"> <br>

The above shows the different commands and options on the `crossroads` tool.

<br>
<img alt ="#" src =".\assets\package_help_result.png">

This above image also shows the help page for the <code>package</code> sub-command of the crossroads tool. We can actually do the same for the other sub-commands on the crossroads tool. <br>
<br>

# Result 

After successfully packaging an application, we get success feedback, otherwise the necessary error message.

<img alt ="#" src = ".\assets\package_success.png"> <br>

Our custom location which has the packaged application <br>

<img alt = "#" src =".\assets\package_output.png"> <br>

## Launching the application
Having packaged javacrossroads, let's try running the created executable by navigating to the output directory and running the command below:

```.\javacrossroads-test.exe```  <br>

Below is the expected output: <br>

<img alt = "#" src = ".\assets\test-javacrossroads.png">
<br>
Voila! There we have it! Same behavior from the original console application.

## Package Inspection

To see the metadata and or additional information in our packaged application, run the command below:

```crossroads inspect --package "javacrossroads-test.exe"```
<br><br>
And the corresponding output below: <br><br>
<img alt = "#" src = ".\assets\javacrossroads-inspect.png">

## Testing our console application

Having packaged our java console app, let's try running the created executable on a different machine which does not have java installed to verify whether the executable created is self-contained or not.
 First we verify if java has been installed locally by running the ```java``` command then we run:

```.\javacrossroads-test.exe``` 

The result is seen below:

<img alt = "#" src = ".\assets\selfcontained_java.png">.

## Conclusion.
 From the image above, it can be deduced that eventhough java is not installed locally we have been able to successfully launch the javacrossroads application which has displayed the "Hello world!" message as expected.





