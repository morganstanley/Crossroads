# Tutorial - Package a python console application using the Crossroads tool packager
As a user  I have a python application on my local windows machine which I would want to share with other developers so they can run on their local linux machine.
This tutorial teaches you how to package a simple python console application with crossroads into a self-contained file. The <code>crossroads</code> tool which is is a NuGet package that is installed from the .NET CLI, will generate a self-contained file, making sure that others can install and run the generated package.

## Pre-requisites
<li>.Net runtime 6.0 or later version</li>
This tutorial uses .NET SDK 6.1, but global tools are available starting in .NET Core SDK 2.1. Local tools are available starting in NET Core SDK 3.0.
<li>Python 3.10.6 or later version</li>
<li>Any python console application</li>
<br>

## What to expect

By the end of this tutorial, it should be expected that:
<li>The python application will be tested locally on a windows machine which has python already installed and the output,"Hello,pythonix crossroads for linux" will be displayed on the console</li>
<li>The application will be packaged with crossroads and a self-contained, file will be generated</li>
<li>The file generated will be run on the windows machine and its expected to display the same output, "Hello,pythonix crossroads for linux"</li>
<li>The executable file will be distributed to linux-based machine</li>
<li>The expected result "Hello,pythonix crossroads for linux" will be displayed after running the executable</li>

## Usage
``` dotnet tool install --global MorganStanley.Crossroads --version 1.0.0 ```

The above command installs the crossroads as a global tool. This will enable you to run the tool from anywhere on your machine.

## Testing our console application

First,let's see the output of the pythoncrossroads console application:
<br>
<img alt="#" src =".\assets\python-linux-output.png">
<br>

let's see the current folder structure of the pythonCrossroads console application

<img alt="#" src=".\assets\pythonfolder_structure.png">

## Packaging our console application
We now head straight to packaging our application and generating the file.

## Command to package the pythonCrossroads console application

```crossroads package --name package_python_for_Linux --command "python3" --location .\OurPythonOutput --targetos linux-x64 --version "2.0" --include "C:/PythonCrossroads/PythonCrossroads" --args "PythonCrossroads/PythonCrossroads.py"```

<img alt="#" src=".\assets\pythonlinux-package-success.png">

Now let's take a look into what each command represents:

<hr>
<code>crossroads</code> is the tool that we installed globally at the beginning of this tutorial and that is the base command we'll use whenever we want to use crossroads to package an application.

<code>package</code> is a sub-command on the crossroads tool. The package sub-command is used to create an executable package. Upon successfully packaging the console application this should generate an executable with a given name, `pythoncrossroads-test` in our case.
We can see a list of commands and options on the crossroads tool by typing `crossroads -h` or just `crossroads`.

`--name or -n`:this command allows us to specify a name for the executable that will be generated.

`--include`: Include internal resource application to be packaged.
The --include command can be used multiple times especially in scenarios where the dependencies needed to run the framework plus the project files are found in two separate file locations.
in this instance since python comes pre-installed on ubuntu, there is no need to include all the oython runtime configuration files as demonstrated on test_case_d.

`--command`:This command can have the alias `-c ` option on the crossroads tool. It allows us to specify the command to run the internal application, which in our case is the `python` executable.

`--location`: This option allows us to set the output file location of the package.

`--targetos`: This option allows us to specify the os platform for which we are packaging our application for.In this instance because we are packaging our application to run on a linux machine hence the target os option "linux-x64" specified. 

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

<img alt="#" src=".\assets\python-linux-success.png">

Our custom location which has the packaged application <br>

<img alt="#" src=".\assets\pythonlinux-package-success.png"> <br>

## Testing our console application

Having packaged our python console app, let's try running the packaged file on a linux based  machine but first we need to make the file executable on the linux machine to enable us run it using the command 

```chmod +x ./package_python_for_Linux```

The result is seen below:
<img alt = "#" src =".\assets\generate-exe.png"> <br>

the results after running the file is seen below:

<img alt = "#" src = ".\assets\python-linux-output-success.png">

## Conclusion.
 From the image above, it can be deduced that we have been able to successfully launch the pythoncrossroads application which has displayed the "Hello,pythonix crossroads for linux" message as expected eventhough the file was generated on a windows machine.



 





