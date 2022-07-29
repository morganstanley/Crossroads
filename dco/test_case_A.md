# Tutorial: Package a .Net console application using the Crossroads tool packager

This tutorial teaches you how to use the crossroads tool packaging manager. The <code>crossroads </code> tool generates an executable which is self-contained, making sure that others can install and run your package without having to install any dependencies. The <code>crossroads</code> tool is a NuGet package that is installed from the .NET CLI.

The console application that we'll package takes a message as input and displays the message along with lines of text that create the image of Morgan Stanley initials.

## Perequisites

<li>.Net SDK  3.1.0 or later version</li>
This tutorial uses .NET SDK 3.1, but global tools are available starting in .NET Core SDK 2.1. Local tools are available starting in .NET Core SDK 3.0.
<li>Any .Net console applicaton</li>
<br>

## Usage

`dotnet tool install -g MorganStanley.Crossroads`
The above command installs the crossroads as a global tool. This will enable you run the tool from any where on your machine.

## Packaging our console app

First of, let's see the output of the console application
<br>
<img alt="#" src =".\assets\project-output.png">
<br>
Let's see the current folder structure of the .Net console application
<img alt="#" src=".\assets\\project-structure.png">
We now head straight to packaging our application and generate the single executable

## Command to package the console application

After installing crossroads as a global tool, it should be acccesible every from your machine console
<code>crossroads --help </code> shuws the various commands and argument needed for packaging.
<img alt="#" src=".\assets\\help.png">

In our case the command below should be okay to package our application into a single executable.
`crossroads package --name test --version 1.0.0 --location "C:\Users\User\Desktop\Packages" --command "Packages\TestApp\TeastApp.exe" --args "crossroads"`

## After packaging

A successful packaged application prints `Package application successfully.` on the console.
Followed by an executable file with name `test` at location `C:\Users\User\Desktop\Packages` as specified during the package generation.

<img alt="#" src=".\assets\\generated-exe.png">

Running the above executable outputs same result as we started
<img alt="#" src =".\assets\project-output.png">

## Advantages of using crossroads

<li>Generate a single application for easy sharing and deployment</li> 
<li>Reduces the size of project drastically to a very minimum file size without losing any aspect of the project</li>
