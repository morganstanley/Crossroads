/*
 * Morgan Stanley makes this available to you under the Apache License,
 * Version 2.0 (the "License"). You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0.
 *
 * See the NOTICE file distributed with this work for additional information
 * regarding copyright ownership. Unless required by applicable law or agreed
 * to in writing, software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
 * or implied. See the License for the specific language governing permissions
 * and limitations under the License.
 */

using Crossroads.Commands;
using Crossroads.Services;
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
using System.Linq;
using System.Threading.Tasks;

namespace Crossroads
{
    class Program
    {
        private static Parser CommandLineParser { get; set; }
        static async Task<int> Main(string[] args)
        {
            CommandLineParser = SetParser(args);

            var rootCommand = CommandLineParser.Configuration.RootCommand;
            var packageCommand = rootCommand.Children.FirstOrDefault(x => x.Name == "package");

            if (args.Any(x => x.Contains(packageCommand.Name)))
            {
                return await ToolMode(args);
            }


            var inspectCommand = rootCommand.Children.FirstOrDefault(x => x.Name == "inspect");
            if (args.Any(x => x.Contains(inspectCommand.Name)))
            {
                await ToolMode(args);
                await LauncherMode(args);
                return 0;
            }
            {
                //launch application
                await LauncherMode(args);
            }

            return 0;
        }

        private static Parser SetParser(string[] args)
        {
           return new CommandLineBuilder()
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
        }
        private async static Task<int> ToolMode(string[] args)
        {

            return await CommandLineParser.InvokeAsync(args);
        }

        private async static Task<int> LauncherMode(string[] args)
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
