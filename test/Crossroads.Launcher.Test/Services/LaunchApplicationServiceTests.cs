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

using Crossroads.Launcher.Services;
using Crossroads.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Threading.Tasks;
using Xunit;

namespace Crossroads.Launcher.Test.Services
{
    public class LaunchApplicationServiceTests
    {
        [Fact]
        public async Task Launch_BadCommand_Win32Exception()
        {
            IFileSystem fileSystem = new MockFileSystem();
            var configuration = new Mock<IConfiguration>();
            var comandSectionMock = new Mock<IConfigurationSection>();
            comandSectionMock.Setup(s => s.Value).Returns("badCommand");
            configuration.Setup(c => c.GetSection("Launcher:Command")).Returns(comandSectionMock.Object);

            var processService = new Mock<IProcessService>();
            processService.Setup(x => x.RunAsync(It.IsAny<ProcessStartInfo>()))
                .ThrowsAsync(new Win32Exception())
                .Verifiable();

            ILaunchApplicationService launchApplicationService = new LaunchApplicationService(configuration.Object, fileSystem, processService.Object);
            await Assert.ThrowsAsync<Win32Exception>(async () => await launchApplicationService.RunAsync());
            processService.Verify();
        }

        [Fact]
        public async Task Launch_Cmd_WithArgs_Success()
        {
            IFileSystem fileSystem = new MockFileSystem();
            var processService = new Mock<IProcessService>();
            processService.Setup(x => x.RunAsync(It.IsAny<ProcessStartInfo>()))
                .Callback<ProcessStartInfo>(x => Assert.Equal("/c echo abc", x.Arguments))
                .ReturnsAsync(0)
                .Verifiable();

            var configuration = new Mock<IConfiguration>();
            var comandSectionMock = new Mock<IConfigurationSection>();
            comandSectionMock.Setup(s => s.Value).Returns("cmd");
            var argsSectionMock = new Mock<IConfigurationSection>();
            argsSectionMock.Setup(s => s.Value).Returns("/c echo abc");
            configuration.Setup(c => c.GetSection("Launcher:Command")).Returns(comandSectionMock.Object);
            configuration.Setup(c => c.GetSection("Launcher:Args")).Returns(argsSectionMock.Object);

            ILaunchApplicationService launchApplicationService = new LaunchApplicationService(configuration.Object, fileSystem, processService.Object);
            var actual = await launchApplicationService.RunAsync();
            Assert.Equal(0, actual);
            processService.Verify();
        }

        [Fact]
        public async Task Launch_Cmd_WithOverideArgs_Success()
        {
            IFileSystem fileSystem = new MockFileSystem();
            var configuration = new Mock<IConfiguration>();
            var comandSectionMock = new Mock<IConfigurationSection>();
            comandSectionMock.Setup(s => s.Value).Returns("cmd");
            var argsSectionMock = new Mock<IConfigurationSection>();
            argsSectionMock.Setup(s => s.Value).Returns("/c echo abc");
            configuration.Setup(c => c.GetSection("Launcher:Command")).Returns(comandSectionMock.Object);
            configuration.Setup(c => c.GetSection("Launcher:Args")).Returns(argsSectionMock.Object);

            var processService = new Mock<IProcessService>();
            processService.Setup(x => x.RunAsync(It.IsAny<ProcessStartInfo>()))
                .Callback<ProcessStartInfo>(x => Assert.Equal("/c echo def", x.Arguments))
                .ReturnsAsync(0)
                .Verifiable();

            ILaunchApplicationService launchApplicationService = new LaunchApplicationService(configuration.Object, fileSystem, processService.Object);
            var actual = await launchApplicationService.RunAsync("/c echo def");
            Assert.Equal(0, actual);
            processService.Verify();
        }

        [Fact]
        public async Task Launch_CommandNull_Exception()
        {
            IFileSystem fileSystem = new MockFileSystem();
            Mock<IConfiguration> configuration = new Mock<IConfiguration>();
            ILaunchApplicationService launchApplicationService = new LaunchApplicationService(configuration.Object, fileSystem, Mock.Of<IProcessService>());
            await Assert.ThrowsAsync<Exception>(async () => await launchApplicationService.RunAsync());
        }

        [Fact]
        public async Task Launch_ArgsInAssets_Success()
        {
            MockFileSystem fileSystem = new MockFileSystem();
            fileSystem.AddFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "assets", "include1", "1.txt"), new MockFileData("abc"));

            var configuration = new Mock<IConfiguration>();
            var comandSectionMock = new Mock<IConfigurationSection>();
            comandSectionMock.Setup(s => s.Value).Returns("cmd");
            var argsSectionMock = new Mock<IConfigurationSection>();
            argsSectionMock.Setup(s => s.Value).Returns(@"/c type .\include1\1.txt");
            configuration.Setup(c => c.GetSection("Launcher:Command")).Returns(comandSectionMock.Object);
            configuration.Setup(c => c.GetSection("Launcher:Args")).Returns(argsSectionMock.Object);

            var processService = new Mock<IProcessService>();
            processService.Setup(x => x.RunAsync(It.IsAny<ProcessStartInfo>()))
                .ReturnsAsync(0)
                .Verifiable();

            ILaunchApplicationService launchApplicationService = new LaunchApplicationService(configuration.Object, fileSystem, processService.Object);
            var actual = await launchApplicationService.RunAsync();
            Assert.Equal(0, actual);
        }

        [Fact]
        public async Task Launch_AppInAssets_Success()
        {
            MockFileSystem fileSystem = new MockFileSystem();
            fileSystem.AddFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "assets", "include1", "1.txt"), new MockFileData("abc"));
            fileSystem.AddFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "assets", "include1", "app.txt"), new MockFileData("abc"));
            
            var configuration = new Mock<IConfiguration>();
            var comandSectionMock = new Mock<IConfigurationSection>();
            comandSectionMock.Setup(s => s.Value).Returns(@".\include\app.exe");
            var argsSectionMock = new Mock<IConfigurationSection>();
            argsSectionMock.Setup(s => s.Value).Returns(@"/c type .\include1\1.txt");
            configuration.Setup(c => c.GetSection("Launcher:Command")).Returns(comandSectionMock.Object);
            configuration.Setup(c => c.GetSection("Launcher:Args")).Returns(argsSectionMock.Object);

            var processService = new Mock<IProcessService>();
            processService.Setup(x => x.RunAsync(It.IsAny<ProcessStartInfo>()))
                .ReturnsAsync(0)
                .Verifiable();

            ILaunchApplicationService launchApplicationService = new LaunchApplicationService(configuration.Object, fileSystem, processService.Object);
            var actual = await launchApplicationService.RunAsync();
            Assert.Equal(0, actual);
        }

        [Fact]
        public async Task Launch_Cmd_WithSingleInclude_WithNonRelativeCmdConfig_Success()
        {
            MockFileSystem fileSystem = new MockFileSystem();
            fileSystem.AddFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "assets", "include1", "1.txt"), new MockFileData("abc"));

            var processService = new Mock<IProcessService>();
            processService.Setup(x => x.RunAsync(It.IsAny<ProcessStartInfo>()))
                .Callback<ProcessStartInfo>(x => Assert.Equal("/c echo abc", x.Arguments))
                .ReturnsAsync(0)
                .Verifiable();

            var configuration = new Mock<IConfiguration>();
            var comandSectionMock = new Mock<IConfigurationSection>();
            comandSectionMock.Setup(s => s.Value).Returns("cmd");
            var argsSectionMock = new Mock<IConfigurationSection>();
            argsSectionMock.Setup(s => s.Value).Returns("/c echo abc");
            var include1SectionMock = new Mock<IConfigurationSection>();
            include1SectionMock.Setup(s => s.Value).Returns("include1");
            var includeSectionMock = new Mock<IConfigurationSection>();
            includeSectionMock.Setup(s => s.GetChildren()).Returns(new List<IConfigurationSection> { include1SectionMock.Object });
            configuration.Setup(c => c.GetSection("Launcher:Command")).Returns(comandSectionMock.Object);
            configuration.Setup(c => c.GetSection("Launcher:Args")).Returns(argsSectionMock.Object);
            configuration.Setup(c => c.GetSection("Launcher:Include")).Returns(includeSectionMock.Object);

            ILaunchApplicationService launchApplicationService = new LaunchApplicationService(configuration.Object, fileSystem, processService.Object);
            var actual = await launchApplicationService.RunAsync();
            Assert.Equal(0, actual);
            processService.Verify();
        }

        [Fact]
        public async Task Launch_Cmd_WithSingleInclude_WithRelativeCmdConfig_Success()
        {
            MockFileSystem fileSystem = new MockFileSystem();
            fileSystem.AddFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "assets", "include1", "1.txt"), new MockFileData("abc"));

            var processService = new Mock<IProcessService>();
            processService.Setup(x => x.RunAsync(It.IsAny<ProcessStartInfo>()))
                .Callback<ProcessStartInfo>(x => Assert.Equal("/c echo abc", x.Arguments))
                .ReturnsAsync(0)
                .Verifiable();

            var configuration = new Mock<IConfiguration>();
            var comandSectionMock = new Mock<IConfigurationSection>();
            comandSectionMock.Setup(s => s.Value).Returns("./include/cmd");
            var argsSectionMock = new Mock<IConfigurationSection>();
            argsSectionMock.Setup(s => s.Value).Returns("/c echo abc");
            var include1SectionMock = new Mock<IConfigurationSection>();
            include1SectionMock.Setup(s => s.Value).Returns("include1");
            var includeSectionMock = new Mock<IConfigurationSection>();
            includeSectionMock.Setup(s => s.GetChildren()).Returns(new List<IConfigurationSection> { include1SectionMock.Object });
            configuration.Setup(c => c.GetSection("Launcher:Command")).Returns(comandSectionMock.Object);
            configuration.Setup(c => c.GetSection("Launcher:Args")).Returns(argsSectionMock.Object);
            configuration.Setup(c => c.GetSection("Launcher:Include")).Returns(includeSectionMock.Object);

            ILaunchApplicationService launchApplicationService = new LaunchApplicationService(configuration.Object, fileSystem, processService.Object);
            var actual = await launchApplicationService.RunAsync();
            Assert.Equal(0, actual);
            processService.Verify();
        }

        [Fact]
        public async Task Launch_Cmd_WithMultipleIncludes_WithRelativeCmdConfig_Success()
        {
            IFileSystem fileSystem = new MockFileSystem();
            var processService = new Mock<IProcessService>();
            processService.Setup(x => x.RunAsync(It.IsAny<ProcessStartInfo>()))
                .Callback<ProcessStartInfo>(x => Assert.Equal("/c echo abc", x.Arguments))
                .ReturnsAsync(0)
                .Verifiable();

            var configuration = new Mock<IConfiguration>();
            var comandSectionMock = new Mock<IConfigurationSection>();
            comandSectionMock.Setup(s => s.Value).Returns("./include1/cmd");
            var argsSectionMock = new Mock<IConfigurationSection>();
            argsSectionMock.Setup(s => s.Value).Returns("/c echo abc");
            var include1SectionMock = new Mock<IConfigurationSection>();
            include1SectionMock.Setup(s => s.Value).Returns("include1");
            var include2SectionMock = new Mock<IConfigurationSection>();
            include2SectionMock.Setup(s => s.Value).Returns("include2");
            var includeSectionMock = new Mock<IConfigurationSection>();
            includeSectionMock.Setup(s => s.GetChildren()).Returns(new List<IConfigurationSection> { include1SectionMock.Object, include2SectionMock.Object });
            configuration.Setup(c => c.GetSection("Launcher:Command")).Returns(comandSectionMock.Object);
            configuration.Setup(c => c.GetSection("Launcher:Args")).Returns(argsSectionMock.Object);
            configuration.Setup(c => c.GetSection("Launcher:Include")).Returns(includeSectionMock.Object);

            ILaunchApplicationService launchApplicationService = new LaunchApplicationService(configuration.Object, fileSystem, processService.Object);
            var actual = await launchApplicationService.RunAsync();
            Assert.Equal(0, actual);
            processService.Verify();
        }

        [Fact]
        public async Task Launch_Cmd_WithMultipleIncludes_NonRelativeCmdConfig_Exception()
        {
            IFileSystem fileSystem = new MockFileSystem();
            var processService = new Mock<IProcessService>();

            var configuration = new Mock<IConfiguration>();
            var comandSectionMock = new Mock<IConfigurationSection>();
            comandSectionMock.Setup(s => s.Value).Returns("cmd");
            var argsSectionMock = new Mock<IConfigurationSection>();
            argsSectionMock.Setup(s => s.Value).Returns("/c echo abc");
            var include1SectionMock = new Mock<IConfigurationSection>();
            include1SectionMock.Setup(s => s.Value).Returns("include1");
            var include2SectionMock = new Mock<IConfigurationSection>();
            include2SectionMock.Setup(s => s.Value).Returns("include2");
            var includeSectionMock = new Mock<IConfigurationSection>();
            includeSectionMock.Setup(s => s.GetChildren()).Returns(new List<IConfigurationSection> { include1SectionMock.Object, include2SectionMock.Object });
            configuration.Setup(c => c.GetSection("Launcher:Command")).Returns(comandSectionMock.Object);
            configuration.Setup(c => c.GetSection("Launcher:Args")).Returns(argsSectionMock.Object);
            configuration.Setup(c => c.GetSection("Launcher:Include")).Returns(includeSectionMock.Object);

            ILaunchApplicationService launchApplicationService = new LaunchApplicationService(configuration.Object, fileSystem, processService.Object);
            await Assert.ThrowsAsync<Exception>(async () => await launchApplicationService.RunAsync());
        }

    }
}
