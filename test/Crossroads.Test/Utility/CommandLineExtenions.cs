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
