using Crossroads.Launcher.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Parsing;
using System.Threading.Tasks;
using Xunit;

namespace Crossroads.Launcher.Commands.Test
{
    public class LauncherInspectCommandTests
    {
        [Fact]
        public async Task LauncherInspectCommand_Success()
        {
            var launcherInspectService = new Mock<ILauncherInspectService>();
            launcherInspectService.Setup(x => x.DisplayOption())
                .Verifiable();

            var command = new LauncherInspectCommand();

            var exitCode = await ExecuteSystemCommand(command, "inspect", (_, services) =>
            {
                services.AddSingleton<ILauncherInspectService>(launcherInspectService.Object);
            });

            launcherInspectService.Verify();
        }

        private async Task<int> ExecuteSystemCommand(Command command, string args, Action<HostBuilderContext, IServiceCollection> configureDelegate)
        {
            var parser = new CommandLineBuilder(command)
                .UseHost(_ => Host.CreateDefaultBuilder(),
                hostBuilder =>
                {
                    hostBuilder.ConfigureServices(configureDelegate);
                }).Build();

            var parseResult = parser.Parse(args);
            var exitCode = await parseResult.InvokeAsync();
            return exitCode;
        }
    }
}
