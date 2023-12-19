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
            AddOption(TargetOsOption);

            Handler = CommandHandler.Create<IHost, PackageOption>(PackageHandler);
        }

        private readonly Option NameOption = new Option(
                   new[] { "--name", "-n" },
                   description: "Set name for rebranding executable",
                   argumentType: typeof(string),
                   arity: ArgumentArity.ExactlyOne)
        {
            IsRequired = true,
        };

        private readonly Option CommandOption = new Option(
                   new[] { "--command", "-c" },
                   description: "Specify command to run the internal application",
                   argumentType: typeof(string),
                   arity: ArgumentArity.ExactlyOne)
        {
            IsRequired = true 
        };
        
        private readonly Option ArgsOption = new Option(
                   new[] { "--args" },
                   description: "Add arguments for the internal application",
                   argumentType: typeof(string), 
                   arity: ArgumentArity.ExactlyOne);

        private readonly Option LocationOption = new Option(
                   new[] { "--location", "-l" }, 
                   description: "Set the output file location of the package", 
                   argumentType: typeof(string), 
                   arity: ArgumentArity.ExactlyOne);

        private readonly Option IconOption = new Option(
                   new[] { "--icon" }, 
                   description: "Set icon for the executable package, should be of '.ico' extension",
                   argumentType: typeof(string),
                   arity: ArgumentArity.ExactlyOne);

        private readonly Option VersionOption = new Option(
                   new[] { "--version" },
                   description: "Set version for the executable package",
                   argumentType: typeof(string),
                   arity: ArgumentArity.ExactlyOne);
       

        private readonly Option IncludeOption = new Option(
                   new[] { "--include" }, 
                   description: "Include internal resource application to be packaged",
                   argumentType: typeof(List<string>), 
                   arity: ArgumentArity.OneOrMore);

        private readonly Option TargetOsOption = new Option(
                   new[] { "--targetos", "-t" },
                   description: "Set runtime identifier for the executable package", 
                   argumentType: typeof(string), 
                   arity: ArgumentArity.ExactlyOne);

        private async Task<int> PackageHandler(IHost host, PackageOption option)
        {
            try
            {
                IPackageApplicationBuilder packageApplication = host.Services.GetRequiredService<IPackageApplicationBuilder>();
                var appPath = await packageApplication.Build(option);
                Console.WriteLine($"Application packaged successfully at {appPath}");
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to package the application. {ex.Message}");
                return 1;
            }
        }
    }
}
