# Tutorial - Package a python console application using the Crossroads tool packager
As a user  I have a python application on my local windows machine which I would want to share with other developers using a local linux machine without them having to install any dependencies or framework inorder to run my application.
This tutorial teaches you how to package a simple python console application with crossroads into a self-contained, executable file. The <code>crossroads</code> tool which is is a NuGet package that is installed from the .NET CLI, will generate a self-contained executable, making sure that others can install and run our package without having to install any dependencies.

## Pre-requisites
<li>.Net runtime 6.0 or later version</li>
This tutorial uses .NET SDK 6.1, but global tools are available starting in .NET Core SDK 2.1. Local tools are available starting in NET Core SDK 3.0.
<li>Python 3.10.6 or later version</li>
<li>Any python console application</li>
<br>

## What to expect

By the end of this tutorial, it should be expected that:
<li>The python application will be tested locally on a windows machine which has python already installed and the output,"Hello,Visual Studio" will be displayed on the console</li>
<li>The application will be packaged with crossroads and a self-contained, executable file will be generated</li>
<li>The executable file generated will be run and its expected to display the same output, "Hello,Visual Studio"</li>
<li>The executable file will be distributed to another local linux-based machine which doesn't have python installed</li>
<li>The expected result "Hello,Visual Studio" will be displayed after running the executable</li>

## Usage
``` dotnet tool install --global MorganStanley.Crossroads --version 1.0.0 ```

The above command installs the crossroads as a global tool. This will enable you to run the tool from anywhere on your machine.

## Testing our console application

First,let's see the output of the pythoncrossroads console application:
<br>
<img alt="#" src ="">
<br>

let's see the current folder structure of the pythonCrossroads console application

<img alt="#" src="">

## Packaging our console application
We now head straight to packaging our application and generating the single executable.

## Command to package the pythonCrossroads console application

```crossroads package --name python-crossroads-test -t linux-x64 --command "Python39_64\python.exe" --location .\OurPythonOutput  --icon .\testicon.ico --version "2.2.2" --include "C:\Program Files (x86)\Microsoft Visual Studio\Shared\Python39_64" --include "C:\PythonCrossroads\PythonCrossroads"  --args "PythonCrossroads\PythonCrossroads.py"```

<img alt="#" src=".\assets\python-package-success.png">

Now let's take a look into what each command represents:

<hr>
<code>crossroads</code> is the tool that we installed globally at the beginning of this tutorial and that is the base command we'll use whenever we want to use crossroads to package an application.

<code>package</code> is a sub-command on the crossroads tool. The package sub-command is used to create an executable package. Upon successfully packaging the console application this should generate an executable with a given name, `pythoncrossroads-test` in our case.
We can see a list of commands and options on the crossroads tool by typing `crossroads -h` or just `crossroads`.

`--name or -n`:this command allows us to specify a name for the executable that will be generated.

`--include`: Include internal resource application to be packaged.
The --include command can be used multiple times especially in scenarios where the dependencies needed to run the framework plus the project files are found in two separate file locations.
This can be seen in this instance where we need all the dependencies in the python folder inorder to run our application as well as the path to the main class.

`--command`:This command can have the alias `-c ` option on the crossroads tool. It allows us to specify the command to run the internal application, which in our case is the `python.exe` executable.

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

<img alt = "#" src =".\assets\python-output.png"> <br>

## Launching the application
Having packaged pythoncrossroads, let's try running the created executable by navigating to the output directory and running the command below:

```.\python-crossroads-test.exe```  <br>

Below is the expected output: <br>

<img alt = "#" src = ".\assets\python-exe-success.png>
<br>
Voila! There we have it! Same behavior from the original console application.

## Package Inspection

To see the metadata and or additional information in our packaged application, run the command below:

```crossroads inspect --package python-crossroads-test.exe```
<br><br>
And the corresponding output below: <br><br>
<img alt = "#" src = ".\assets\python-crossroads-inspect.png">

## Testing our console application

Having packaged our python console app, let's try running the created executable on a different machine which does not have python installed to verify whether the executable created is self-contained or not.
 First we verify if python has been installed locally by running the ```python --version``` command then we run:

```.\python-crossroads-test.exe``` 

The result is seen below:

<img alt = "#" src = ".\assets\self-contained-python.png">

## Conclusion.
 From the image above, it can be deduced that eventhough python is not installed locally we have been able to successfully launch the pythoncrossroads application which has displayed the "Hello world!" message as expected,hence during packaging,the executable file generated was self-contained which is also verifiable in the assets directory in the temp folder as shown in the image below.

 <img alt = "#" src = ".\assets\python-temp-folder.png">

 





