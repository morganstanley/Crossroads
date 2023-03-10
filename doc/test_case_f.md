# Tutorial - Package a python console application using the Crossroads tool packager
As a user  I have a python application on my local linux machine which I would want to share with other developers so they can run on their local windows machine.
This tutorial teaches you how to package a simple python console application with crossroads into a self-contained executable file. The <code>crossroads</code> tool which is is a NuGet package that is installed from the .NET CLI, will generate a self-contained executable file, making sure that others can install and run the generated package.

## Pre-requisites
<li>ubuntu 22.04 LTS</li>
This tutorial uses .NET SDK 6.1, but global tools are available starting in .NET Core SDK 2.1. Local tools are available starting in NET Core SDK 3.0.
<li>Python 3.10.6 or later version</li>
<li>dotnet 6.1</li>
<br>

## What to expect

By the end of this tutorial, it should be expected that:
<li>The python application will be tested locally on a linux machine which has python already installed and the output,"Hello,pythonix crossroads for linux" will be displayed on the console</li>
<li>The application will be packaged with crossroads and a self-contained, executable file will be generated</li>
<li>The executable file will be distributed to windows-based machine</li>
<li>The expected result "Hello,pythonix crossroads for linux" will be displayed after running the executable on the windows machine</li>

## Usage
```sh
 dotnet tool install --global MorganStanley.Crossroads --version 1.0.0 
```

The above command installs the crossroads as a global tool. This will enable you to run the tool from anywhere on your machine.

## Testing our console application

First,let's see the output of the pythoncrossroads console application:
<br>
<img alt="#" src =".\assets\linuxpython.png">
<br>

## Packaging our console application
We now head straight to packaging our application and generating the executable file.

## Command to package the pythonCrossroads console application

```sh
crossroads package --name package-python-for-windows --command "python3" --targetos win-x64 --include "../script" --args "script/crosspy.py" --location "../result-output"
```

<img alt="#" src=".\assets\pythonforwindows.png">

Now let's take a look into what each command represents:

<hr>
<code>crossroads</code> is the tool that we installed globally at the beginning of this tutorial and that is the base command we'll use whenever we want to use crossroads to package an application.

<code>package</code> is a sub-command on the crossroads tool. The package sub-command is used to create an executable package. Upon successfully packaging the console application this should generate an executable with a given name, `pythoncrossroads-test` in our case.
We can see a list of commands and options on the crossroads tool by typing `crossroads -h` or just `crossroads`.

`--name or -n`:this command allows us to specify a name for the executable that will be generated.

`--include`: Include internal resource application to be packaged.
The --include command can be used multiple times especially in scenarios where the dependencies needed to run the framework plus the project files are found in two separate file locations.
in this instance since python comes pre-installed on ubuntu, there is no need to include all the python runtime configuration files as demonstrated on test_case_d.

`--command`:This command can have the alias `-c ` option on the crossroads tool. It allows us to specify the command to run the internal application, which in our case is the `python` executable.

`--location`: This option allows us to set the output file location of the package.

`--targetos`: This option allows us to specify the os platform for which we are packaging our application for.In this instance because we are packaging our application to run on a windows machine hence the target os option "win-x64" specified. 

Currently our application does not support the version and icon command on the linux environment.Hence a display message "Failed to package the application. Version or Icon is not required." will be displayed when added as part of commands when packaging.

# Result 

After successfully packaging an application, we get success feedback, otherwise the necessary error message.

<img alt="#" src=".\assets\python-linux-success.png">

## Testing our console application

Having packaged our python console app, let's test it by running the packaged file on a windows machine.But first lets have a look at the result-output folder and confirm that the executable file was generated.
The result is seen below:

<img alt = "#" src = ".\assets\linux-resultoutput.png">

Now we distribute the executable file to a windows machine and run it.Here in the downloads folder we have the exe file:
<img alt = "#" src =".\assets\linuxwindowsresult.png"> <br>

the results after running the file is seen below:

<img alt = "#" src = ".\assets\packagepythonforwindows.png">

## Conclusion.
 From the image above, it can be deduced that we have been able to package a python application on a linux machine for the windows os, an executable was generated and the eexecutable has been launched succesfully with the expected result "Hello,pythonix crossroads for linux" displayed.



 





