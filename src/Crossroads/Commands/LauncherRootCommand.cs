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
using Microsoft.Extensions.Logging;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace Crossroads.Commands
{
    public class LauncherRootCommand : RootCommand
    {
        public LauncherRootCommand()
        {
            AddOption(argsOption);
            Handler = CommandHandler.Create<IHost, string>(LauncherApplicationHandler);
        }

        private async Task<int> LauncherApplicationHandler(IHost host, string args)
        {
            var logger = host.Services.GetRequiredService<ILogger<LauncherRootCommand>>();
            try
            {
                var detectService = host.Services.GetRequiredService<IQueryRunningModeService>();
                switch (detectService.Query())
                {
                    case RunningMode.Package:
                        throw new NotImplementedException();
                        break;
                    case RunningMode.Launch:
                        var launcherService = host.Services.GetRequiredService<ILaunchApplicationService>();
                        return await launcherService.RunAsync(args);
                    default:
                        throw new InvalidOperationException();
                }
            }
            catch (Exception e)
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
