using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Parsing;
using System.Threading.Tasks;

namespace Crossroads.Test.Utility
{
    public static class CommandLineExtenions
    {
        public static async Task<int> ExecuteSystemCommand(this Command command, string args, Action<HostBuilderContext, IServiceCollection> configureDelegate)
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
