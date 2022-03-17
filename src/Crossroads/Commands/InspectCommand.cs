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
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace Crossroads.Commands
{
    public class InspectCommand : Command
    {
        public InspectCommand() :
            base("inspect", "View metadata of a generated package")
        {
            AddOption(PackageOption);

            Handler = CommandHandler.Create<IHost, string, IConsole>(async (host, package,console) =>
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