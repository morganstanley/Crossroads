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
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Threading.Tasks;
using Xunit;

namespace Crossroads.Test.Services
{
    public class LaunchApplicationServiceTests
    {
        [Fact]
        public async Task Luanch_BadCommand_Win32Exception()
        {
            IFileSystem fileSystem = new MockFileSystem();
            IConfiguration configuration = Mock.Of<IConfiguration>(
                x => x["Launcher:Command"] == "badCommand"
                );
            var processService = new Mock<IProcessService>();
            processService.Setup(x => x.RunAsync(It.IsAny<ProcessStartInfo>()))
                .ThrowsAsync(new Win32Exception())
                .Verifiable();

            ILaunchApplicationService launchApplicationService = new LaunchApplicationService(configuration, fileSystem, processService.Object);
            await Assert.ThrowsAsync<Win32Exception>(async () => await launchApplicationService.RunAsync());
            processService.Verify();
        }

        [Fact]
        public async Task Luanch_Cmd_WithArgs_Success()
        {
            IFileSystem fileSystem = new MockFileSystem();
            IConfiguration configuration = Mock.Of<IConfiguration>(
                x => x["Launcher:Command"] == "cmd"
                && x["Launcher:Args"] == "/c echo abc"
                );
            var processService = new Mock<IProcessService>();
            processService.Setup(x => x.RunAsync(It.IsAny<ProcessStartInfo>()))
                .Callback<ProcessStartInfo>(x => Assert.Equal("/c echo abc", x.Arguments))
                .ReturnsAsync(0)
                .Verifiable();
            ILaunchApplicationService launchApplicationService = new LaunchApplicationService(configuration, fileSystem, processService.Object);
            var actual = await launchApplicationService.RunAsync();
            Assert.Equal(0, actual);
            processService.Verify();
        }

        [Fact]
        public async Task Luanch_Cmd_WithOverideArgs_Success()
        {
            IFileSystem fileSystem = new MockFileSystem();
            IConfiguration configuration = Mock.Of<IConfiguration>(
                x => x["Launcher:Command"] == "cmd"
                && x["Launcher:Args"] == "/c echo abc"
                );
            var processService = new Mock<IProcessService>();
            processService.Setup(x => x.RunAsync(It.IsAny<ProcessStartInfo>()))
                .Callback<ProcessStartInfo>(x => Assert.Equal("/c echo def", x.Arguments))
                .ReturnsAsync(0)
                .Verifiable();
            ILaunchApplicationService launchApplicationService = new LaunchApplicationService(configuration, fileSystem, processService.Object);
            var actual = await launchApplicationService.RunAsync("/c echo def");
            Assert.Equal(0, actual);
            processService.Verify();
        }

        [Fact]
        public async Task Luanch_CommandNull_Exception()
        {
            IFileSystem fileSystem = new MockFileSystem();
            IConfiguration configuration = Mock.Of<IConfiguration>();
            ILaunchApplicationService launchApplicationService = new LaunchApplicationService(configuration, fileSystem, Mock.Of<IProcessService>());
            await Assert.ThrowsAsync<Exception>(async () => await launchApplicationService.RunAsync());
        }

        [Fact]
        public async Task Luanch_ArgsInAssets_Success()
        {
            MockFileSystem fileSystem = new MockFileSystem();
            fileSystem.AddFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "assets", "include1", "1.txt"), new MockFileData("abc"));
            IConfiguration configuration = Mock.Of<IConfiguration>(
                x => x["Launcher:Command"] == "cmd"
                && x["Launcher:Args"] == @"/c type .\include1\1.txt"
                );
            var processService = new Mock<IProcessService>();
            processService.Setup(x => x.RunAsync(It.IsAny<ProcessStartInfo>()))
                .ReturnsAsync(0)
                .Verifiable();
            ILaunchApplicationService launchApplicationService = new LaunchApplicationService(configuration, fileSystem, processService.Object);
            var actual = await launchApplicationService.RunAsync();
            Assert.Equal(0, actual);
        }

        [Fact]
        public async Task Luanch_AppInAssets_Success()
        {
            MockFileSystem fileSystem = new MockFileSystem();
            fileSystem.AddFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "assets", "include1", "1.txt"), new MockFileData("abc"));
            fileSystem.AddFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "assets", "include1", "app.txt"), new MockFileData("abc"));
            IConfiguration configuration = Mock.Of<IConfiguration>(
                x => x["Launcher:Command"] == @".\include\app.exe"
                && x["Launcher:Args"] == @"/c type .\include1\1.txt"
                );
            var processService = new Mock<IProcessService>();
            processService.Setup(x => x.RunAsync(It.IsAny<ProcessStartInfo>()))
                .ReturnsAsync(0)
                .Verifiable();
            ILaunchApplicationService launchApplicationService = new LaunchApplicationService(configuration, fileSystem, processService.Object);
            var actual = await launchApplicationService.RunAsync();
            Assert.Equal(0, actual);
        }

    }
}
