using Crossroads.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace Crossroads.Commands
{
    public class InspectCommand : Command
    {
        public InspectCommand():
            base("inspect", "View metadata of a generated package")
        {
            AddOption(PackageOption);

            Handler = CommandHandler.Create<IHost, string, IConsole>(async (host, package, console) =>
            {
                var inspectService = host.Services.GetRequiredService<IInspectService>();
                var consoleOutput = await inspectService.InspectLauncherPackage(package);
                Console.WriteLine(consoleOutput);
            });
        }

        private Option PackageOption = new Option<string>(
                   new[] { "--package" },
                    description: "Specify path to the generated package"
                 )
        {
            IsRequired = true
        };
    }

}