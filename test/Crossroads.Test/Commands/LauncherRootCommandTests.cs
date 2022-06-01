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
using Crossroads.Test.Utility;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Parsing;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Crossroads.Test.Commands
{
    public class LauncherRootCommandTests
    {
        [Fact]
        public async Task CrossroadsRootCommand_Run_WhenModeIsLaunch_Success()
        {
            var launchApp = new Mock<ILaunchApplicationService>();
            var queryRunningModeService = new Mock<IQueryRunningModeService>();
            var displayHelpPage = new Mock<IDisplayHelpPage>();

            launchApp.Setup(x => x.RunAsync(It.IsAny<string>()))
                .ReturnsAsync(0)
                .Verifiable();

            queryRunningModeService.Setup(x => x.Query())
                .Returns(RunningMode.Launch)
                .Verifiable();

            var parser = new CommandLineBuilder(new CrossroadsRootCommand())
               .UseHost(_ => Host.CreateDefaultBuilder(),
               hostBuilder =>
               {
                   hostBuilder.ConfigureServices(services =>
                   {
                       services.AddSingleton(queryRunningModeService.Object);
                       services.AddSingleton(launchApp.Object);
                   });
               }).Build();

            var parserResult = parser.Parse("chris.exe");
            var action = await parserResult.InvokeAsync();

            Assert.Equal(0, action);
            launchApp.Verify();
        }

        [Fact]
        public async Task CrossroadsRootCommand_Run_WhenModeIsLaunch_FailedWithException()
        {
            var launchApp = new Mock<ILaunchApplicationService>();
            var queryRunningModeService = new Mock<IQueryRunningModeService>();
            var displayHelpPage = new Mock<IDisplayHelpPage>();

            launchApp.Setup(x => x.RunAsync(It.IsAny<string>()))
                .ThrowsAsync(Mock.Of<ArgumentNullException>())
                .Verifiable();

            queryRunningModeService.Setup(x => x.Query())
                .Returns(RunningMode.Launch)
                .Verifiable();

            var parser = new CommandLineBuilder(new CrossroadsRootCommand())
               .UseHost(_ => Host.CreateDefaultBuilder(),
               hostBuilder =>
               {
                   hostBuilder.ConfigureServices(services =>
                   {
                       services.AddSingleton(queryRunningModeService.Object);
                       services.AddSingleton(launchApp.Object);
                   });
               }).Build();

            var parserResult = parser.Parse(null);
            var action = await parserResult.InvokeAsync();

            Assert.Equal(1,action);
            launchApp.Verify();
        }

        [Fact]
        public async Task CrossroadsRootCommand_Run_WhenModeIsPackage_Success()
        {
            var queryRunningModeService = new Mock<IQueryRunningModeService>();
            var displayHelpPage = new Mock<IDisplayHelpPage>();

            queryRunningModeService.Setup(x => x.Query())
                .Returns(RunningMode.Package)
                .Verifiable();

            displayHelpPage.Setup(x => x.GetHelpPage(It.IsAny<RootCommand>()))
                .ReturnsAsync(0)
                .Verifiable();

            var parser = new CommandLineBuilder(new CrossroadsRootCommand())
               .UseHost(_ => Host.CreateDefaultBuilder(),
               hostBuilder =>
               {
                   hostBuilder.ConfigureServices(services =>
                   {
                       services.AddSingleton(queryRunningModeService.Object);
                       services.AddSingleton(displayHelpPage.Object);
                   });
               }).Build();

            var parserResult = parser.Parse(@".\Crossroads.exe");
            var exitCode = await parserResult.InvokeAsync();

            Assert.Equal(0,exitCode);
            displayHelpPage.Verify();
        }

        [Fact]
        public async Task CrossroadsRootCommand_Run_WhenModeIsPackage_Fail()
        {
            var queryRunningModeService = new Mock<IQueryRunningModeService>();
            var displayHelpPage = new Mock<IDisplayHelpPage>();

            queryRunningModeService.Setup(x => x.Query())
                .Returns(RunningMode.Package)
                .Verifiable();

            displayHelpPage.Setup(x => x.GetHelpPage(It.IsAny<RootCommand>()))
                .ReturnsAsync(1)
                .Verifiable();

            var parser = new CommandLineBuilder(new CrossroadsRootCommand())
               .UseHost(_ => Host.CreateDefaultBuilder(),
               hostBuilder =>
               {
                   hostBuilder.ConfigureServices(services =>
                   {
                       services.AddSingleton(queryRunningModeService.Object);
                       services.AddSingleton(displayHelpPage.Object);
                   });
               }).Build();

            var parserResult = parser.Parse("randomfile");
            var exitCode = await parserResult.InvokeAsync();

            Assert.Equal(1, exitCode);
        }
    }
}
