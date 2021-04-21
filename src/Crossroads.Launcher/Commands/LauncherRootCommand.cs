using Crossroads.Launcher.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace Crossroads.Launcher.Commands
{
    public class LauncherRootCommand: RootCommand
    {
        public LauncherRootCommand()
        {
            AddOption(argsOption);
            this.Handler = CommandHandler.Create<IHost, string>(LauncherApplicationHandler);
        }

        private async Task<int> LauncherApplicationHandler(IHost host, string args)
        {
            var logger = host.Services.GetRequiredService<ILogger<LauncherRootCommand>>();
            try
            {
                var launcherService = host.Services.GetRequiredService<ILaunchApplicationService>();
                return await launcherService.RunAsync(args);

            }
            catch(Exception e)
            {
                logger.LogError(e.Message);
            }
            return 1;
        }

        private readonly Option argsOption = new Option<string>(new string[] { "--args" }, "Override arguments.")
        {
            Argument = new Argument<string>
            {
                Arity = ArgumentArity.ExactlyOne
            }
        };
    }
}
