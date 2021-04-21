using Crossroads.Core;
using Crossroads.Launcher.Commands;
using Crossroads.Launcher.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Parsing;
using System.Diagnostics;
using System.IO.Abstractions;
using System.Threading.Tasks;

namespace Crossroads.Launcher
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            // Debugger.Launch();
            var parser = new CommandLineBuilder(new LauncherRootCommand())
                .AddCommand(new LauncherInspectCommand())
                .UseDefaults()
                .UseHost(_ => Host.CreateDefaultBuilder(args),
                    hostBuilder =>
                    {
                        hostBuilder
                            .ConfigureAppConfiguration((hostingContext, config) =>
                            {
                                config.AddJsonFile($"{AppDomain.CurrentDomain.BaseDirectory}appsettings.json", optional: false, reloadOnChange: true);
                            })
                            .ConfigureServices((context, services) =>
                            {
                                services.AddTransient<IFileSystem, FileSystem>();
                                services.AddTransient<ILauncherInspectService, LauncherInspectService>();
                                services.AddTransient<ILaunchApplicationService, LaunchApplicationService>();
                                services.AddTransient<IProcessService, ProcessService>();
                            });
                    })
                .Build();

            return await parser.InvokeAsync(args);
        }
    }
}
