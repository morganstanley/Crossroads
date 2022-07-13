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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using System;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Parsing;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Crossroads.Test.Commands
{
    public class InspectCommandTests
    {
        [Fact]
        public async Task Inspect_Success()
        {
            StringWriter consoleOutWriter = new StringWriter();
            Console.SetOut(consoleOutWriter);

            string expected = "Good Console Output";
            var inspectService = new Mock<IInspectService>();
            inspectService.Setup(service => service.InspectLauncherPackage(It.IsAny<string>()))
                .ReturnsAsync(expected)
                .Verifiable();
            var parser = new CommandLineBuilder(new InspectCommand())
                .UseHost(_ => Host.CreateDefaultBuilder(),
                hostBuilder =>
                {
                    hostBuilder.ConfigureServices(services =>
                    {
                        services.AddSingleton<IInspectService>(inspectService.Object);
                    });
                }).Build();

            var parseResult = parser.Parse("inspect --package goodpackage");
            var exitCode = await parseResult.InvokeAsync();
            var actual = consoleOutWriter.ToString();
            Assert.Contains(expected, actual);
            inspectService.Verify();
        }
    }
}
