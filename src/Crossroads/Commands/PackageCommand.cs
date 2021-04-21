using Crossroads.Core;
using Crossroads.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace Crossroads.Commands
{
    public class PackageCommand : Command
    {
        public PackageCommand()
            : base("package", "Create executable package.")
        {
            AddOption(NameOption);
            AddOption(CommandOption);
            AddOption(ArgsOption);
            AddOption(LocationOption);
            AddOption(IconOption);
            AddOption(VersionOption);
            AddOption(IncludeOption);

            Handler = CommandHandler.Create<IHost, PackageOption>(PackageHandler);
        }

        private readonly Option NameOption = new Option<string>(
                   new[] { "--name", "-n" },
                    description: "Set name for rebranding executable"
            );

        private readonly Option CommandOption = new Option<string>(
                    new[] { "--command", "-c" },
                    description: "Specify command to run the internal application")
        {
            IsRequired = true,

            Argument = new Argument<string>
            {
                Arity = ArgumentArity.ExactlyOne
            }
        };

        private readonly Option ArgsOption = new Option(
                    new[] { "--args"},
                    description: "Add arguments for the internal application")
        {
            Argument = new Argument<string>
            {
                Arity = ArgumentArity.ExactlyOne
            }
        };

        private readonly Option LocationOption = new Option<string>(
                new[] { "--location", "-l"},
                 "Set the output file location of the package")
        {
            Argument = new Argument<string>
            {
                Arity = ArgumentArity.ExactlyOne
            }
        };

        private readonly Option IconOption = new Option<string>(
                    new[] { "--icon"},
                    description: "Set icon for the executable package, should be of '.ico' extension")
        {
            Argument = new Argument<string>
            {
                Arity = ArgumentArity.ExactlyOne
            }
        };

        private readonly Option VersionOption = new Option<string>(
                new[] { "--version" },
                 description: "Set version for the executable package"
        );

        private readonly Option IncludeOption = new Option(
                 new[] { "--include" },
                  description: "Include internal resource application to be packaged")
        {
            Argument = new Argument<List<string>>
            {
                Arity = ArgumentArity.OneOrMore
            }
        };

        private async Task<int> PackageHandler(IHost host, PackageOption option)
        {
            try
            {
                IPackageApplicationBuilder packageApplication = host.Services.GetRequiredService<IPackageApplicationBuilder>();
                await packageApplication.Build(option);
                Console.WriteLine("Package application successfully.");
                return 0;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Failed to package the application.");
                Console.WriteLine(ex.Message);
                return 1;
            }
        }
    }
}
