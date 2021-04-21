using Crossroads.Commands;
using Crossroads.Core;
using Crossroads.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Parsing;
using System.IO.Abstractions;
using System.Threading.Tasks;

namespace Crossroads
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var parser = new CommandLineBuilder()
                .UseDefaults()
                .UseHost(_ => Host.CreateDefaultBuilder(args),
                    hostBuilder =>
                    {
                        hostBuilder.ConfigureAppConfiguration((hostingContext, config) =>
                        {
                            config.AddJsonFile($"{AppDomain.CurrentDomain.BaseDirectory}appsettings.json", optional: false, reloadOnChange: true);
                        })
                        .ConfigureServices(services =>
                        {
                            services.AddSingleton<IFileSystem, FileSystem>();
                            services.AddTransient<IPackageApplicationBuilder, PackageApplicationBuilder>();
                            services.AddTransient<IAppHostService, AppHostService>();
                            services.AddTransient<IResourcesAssemblyBuilder, ResourcesAssemblyBuilder>();
                            services.AddTransient<ILauncherAppsettingsFileService, LauncherAppsettingsFileService>();
                            services.AddTransient<IProcessService, ProcessService>();
                            services.AddTransient<IInspectService, InspectService>();
                        });
                    })
                .AddCommand(new PackageCommand())
                .AddCommand(new InspectCommand())
                .Build();

            return await parser.InvokeAsync(args);
        }
    }
}