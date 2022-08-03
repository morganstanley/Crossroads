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
        static async Task<int> Main(string[] args)
        {
            var parser = new CommandLineBuilder(new CrossroadsRootCommand())
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
                                   services.AddTransient<IPackageApplicationBuilder, PackageApplicationBuilder>();
                                   services.AddTransient<ILauncherAppsettingsFileService, LauncherAppsettingsFileService>();
                                   services.AddTransient<IInspectService, InspectService>();
                                   services.AddTransient<IAppHostService, AppHostService>();
                                   services.AddTransient<IResourcesAssemblyBuilder, ResourcesAssemblyBuilder>();
                                   services.AddTransient<IProcessService, ProcessService>();
                                   services.AddTransient<IQueryRunningModeService, QueryRunningModeService>();
                                   services.AddTransient<IDisplayHelpPage, DisplayHelpPage>();
                               });
                       })
                   .AddCommand(new LauncherInspectCommand())
                   .AddCommand(new PackageCommand())
                   .AddCommand(new InspectCommand())
                   .Build();

            return await parser.InvokeAsync(args);
        }
    }
}
