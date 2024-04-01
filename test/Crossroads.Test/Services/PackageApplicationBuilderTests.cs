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
using Crossroads.Test.Utility;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Xunit;

namespace Crossroads.Test.Services
{
    public class PackageApplicationBuilderTests
    {
        [Fact]
        public async Task Build_NoName_ArgumentExcetpion()
        {
            using var packageApplicationBuilder = GetPackageApplicationBuilder();
            var option = new PackageOption();
            await Assert.ThrowsAsync<ArgumentException>(async () => await packageApplicationBuilder.Build(option));
        }

         [PlatformRestrictedFact(windows: true)]
        public async Task Build_OnWindows_InvalidInclude_ArgumentException()
        {
            using var packageApplicationBuilder = GetPackageApplicationBuilder();
            var option = new PackageOption
            {
                Name = "testapp",
                Command = "Notepad",
                Version = "3.0.1.0",
                Include = new[] { @".\assets\invalidinclude" },
                TargetOs = AppHostService.WIN_RID
            };
            await Assert.ThrowsAsync<ArgumentException>(async () => await packageApplicationBuilder.Build(option));
        }

         [PlatformRestrictedFact(linux: true)]
        public async Task Build_OnLinux_InvalidInclude_ArgumentException()
        {
            using var packageApplicationBuilder = GetPackageApplicationBuilder();
            var option = new PackageOption
            {
                Name = "testapp",
                Command = "Notepad",
                Include = new[] { @"./assets/invalidinclude" },
                TargetOs = AppHostService.LINUX_RID
            };
            await Assert.ThrowsAsync<ArgumentException>(async () => await packageApplicationBuilder.Build(option));
        }
        
        [PlatformRestrictedFact(windows: true)]
        public async Task Build_OnWindows_InvalidIncludes_ArgumentException()
        {
            using var packageApplicationBuilder = GetPackageApplicationBuilder();
            var option = new PackageOption
            {
                Name = "testapp",
                Command = "Notepad",
                Version = "3.0.1.0",
                Include = new[] { @".\assets\invalidinclude", @".\assets\invalidinclude2" },
                TargetOs = AppHostService.WIN_RID
            };
            await Assert.ThrowsAsync<AggregateException>(async () => await packageApplicationBuilder.Build(option));
        }

        [PlatformRestrictedFact(linux: true)]
        public async Task Build_OnLinux_InvalidIncludes_ArgumentException()
        {
            using var packageApplicationBuilder = GetPackageApplicationBuilder();
            var option = new PackageOption
            {
                Name = "testapp",
                Command = "python3",
                Include = new[] { @"./assets/invalidinclude", @"./assets/invalidinclude2" },
                TargetOs = AppHostService.LINUX_RID
            };
            await Assert.ThrowsAsync<AggregateException>(async () => await packageApplicationBuilder.Build(option));
        }

        [Fact]
        public async Task Build_OptionNull_Argumentexception()
        {
            using var packageApplicationBuilder = GetPackageApplicationBuilder();
            await Assert.ThrowsAsync<ArgumentException>(async () => await packageApplicationBuilder.Build(null));
        }

        [PlatformRestrictedFact(windows: true)]
        public async Task Build_OnWindows_WithValidInclude_Success()
        {
            using var packageApplicationBuilder = GetPackageApplicationBuilder();
            var option = new PackageOption
            {
                Name = "testapp",
                Command = "Notepad",
                Version = "3.0.1.0",
                Include = new string[] { @"assets\include" },
                TargetOs = AppHostService.WIN_RID
            };
            await packageApplicationBuilder.Build(option);
        }

        [PlatformRestrictedFact(linux: true)]
        public async Task Build_OnLinux_WithValidInclude_Success()
        {
            using var packageApplicationBuilder = GetPackageApplicationBuilder();
            var option = new PackageOption
            {
                Name = "testapp",
                Command = "python3",
                Version = "3.0.1.0",
                Include = new string[] { @"assets/include" },
                TargetOs = AppHostService.LINUX_RID
            };
            await packageApplicationBuilder.Build(option);
        }

        [Fact]
        public async Task Build_Dispose_WorkingDirDeleteFailure__ResolveException()
        {
            var fileSystem = new Mock<IFileSystem>();
            fileSystem.Setup(x => x.DirectoryInfo.New(It.IsAny<string>())).Returns(It.IsAny<DirectoryInfoBase>());
            fileSystem.Setup(x => x.DirectoryInfo.New(It.IsAny<string>()).GetDirectories()).Returns(It.IsAny<DirectoryInfoBase[]>());
            fileSystem.Setup(x => x.DirectoryInfo.New(It.IsAny<string>()).GetFiles()).Returns(Array.Empty<FileInfoBase>);
            fileSystem.Setup(x => x.DirectoryInfo.New(It.IsAny<string>()).GetDirectories()).Returns(Array.Empty<DirectoryInfoBase>);
            fileSystem.Setup(x => x.Directory.Exists(It.IsAny<string>())).Returns(true);
            fileSystem.Setup(x => x.Directory.Delete(It.IsAny<string>(), It.IsAny<bool>())).Throws(new Exception());
            var resource = new Mock<IResourcesAssemblyBuilder>();
            var appsettingsFile = new Mock<ILauncherAppsettingsFileService>();
            var appHostService = new Mock<IAppHostService>();
            var hostOsService = new Mock<IHostOsDetectionService>();
            hostOsService.Setup(x => x.IsVersionIconSupported(It.IsAny<PackageOption>())).Returns(true);
            var logger = new Mock<ILogger<PackageApplicationBuilder>>();
            var packageApp = new PackageApplicationBuilder(fileSystem.Object, resource.Object, appsettingsFile.Object, appHostService.Object, logger.Object, hostOsService.Object);

            var option = new PackageOption
            {
                Name = "testapp",
                Command = "Notepad",
                Version = "3.0.1.0",
                Include = null,
                TargetOs = AppHostService.WIN_RID
            };

            await packageApp.Build(option);
            packageApp.Dispose();
        }

        [PlatformRestrictedFact(windows: true)]
        public async Task Build_OnWindows_NoTargetOs_AutoDetectWinOs_Success()
        {
            using var packageApplicationBuilder = GetPackageApplicationBuilder();
            var option = new PackageOption
            {
                Name = "testapp",
                Command = "Notepad",
                Version = "3.0.1.0",
                Include = new string[] { @"assets\include" },
                TargetOs = null
            };
            await packageApplicationBuilder.Build(option);
        }

        [PlatformRestrictedFact(linux: true)]
        public async Task Build_OnLinux_NoTargetOs_AutoDetectWinOs_Success()
        {
            using var packageApplicationBuilder = GetPackageApplicationBuilder();
            var option = new PackageOption
            {
                Name = "testapp",
                Command = "Notepad",
                Version = "3.0.1.0",
                Include = new string[] { @"assets/include" },
                TargetOs = null
            };
            await packageApplicationBuilder.Build(option);
        }

        private PackageApplicationBuilder GetPackageApplicationBuilder()
        {
            var fileSystem = new MockFileSystem();
            var platformResourcePath = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? 
                @"assets\include\include2\file1.txt" : 
                @"assets/include/include2/file1.txt";
            fileSystem.AddFile(platformResourcePath, new MockFileData("abc"));
            fileSystem.AddDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Crossroads.Launcher", AppHostService.WIN_RID));
            fileSystem.AddDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Crossroads.Launcher", AppHostService.LINUX_RID));
            var resource = new Mock<IResourcesAssemblyBuilder>();
            var appsettingsFile = new Mock<ILauncherAppsettingsFileService>();
            var appHostService = new Mock<IAppHostService>();
            var hostOsService = new Mock<IHostOsDetectionService>();
            hostOsService.Setup(x => x.GetTargetOsRid()).Returns(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? AppHostService.WIN_RID : AppHostService.LINUX_RID);
            hostOsService.Setup(x => x.IsVersionIconSupported(It.IsAny<PackageOption>())).Returns(true);

            var logger = new Mock<ILogger<PackageApplicationBuilder>>();

            return new PackageApplicationBuilder(fileSystem, resource.Object, appsettingsFile.Object, appHostService.Object, logger.Object, hostOsService.Object);
        }

        [Fact]
        public async Task Build_OnUnsupportedHostOs_AutoDetectOs_ArgumentException()
        {
            var option = new PackageOption { Name = "testapp", TargetOs = null };
            var fileSystem = new MockFileSystem();
            var resource = new Mock<IResourcesAssemblyBuilder>();
            var appsettingsFile = new Mock<ILauncherAppsettingsFileService>();
            var appHostService = new Mock<IAppHostService>();
            var logger = new Mock<ILogger<PackageApplicationBuilder>>();
            var hostOsService = new Mock<IHostOsDetectionService>();
            hostOsService.Setup(x => x.GetTargetOsRid()).Throws(new ArgumentException())
                .Verifiable();

            using var packageApplicationBuilder = new PackageApplicationBuilder(fileSystem, resource.Object, appsettingsFile.Object, appHostService.Object, logger.Object, hostOsService.Object);

            await Assert.ThrowsAsync<ArgumentException>(async () => await packageApplicationBuilder.Build(option));
            hostOsService.Verify();
        }

        [PlatformRestrictedFact(windows: true)]
        public async Task Build_OnWindows_No_Duplicate_Extension_Success()
        {
            using var packageApplicationBuilder = GetPackageApplicationBuilder();
            var option = new PackageOption
            {
                Name = "testapp.exe",
                Command = "Notepad",
                Version = "3.0.1.0",
                Include = new string[] { @"assets\include" },
                TargetOs = AppHostService.WIN_RID
            };
            await packageApplicationBuilder.Build(option);
        }

        [PlatformRestrictedFact(windows: true)]
        public async Task Build_OnWindows_ForLinux_No_Duplicate_Extension_Success()
        {
            using var packageApplicationBuilder = GetPackageApplicationBuilder();
            var option = new PackageOption
            {
                Name = "testapp.exe",
                Command = "Notepad",
                Version = "3.0.1.0",
                Include = new string[] { @"assets\include" },
                TargetOs = AppHostService.LINUX_RID
            };
            await packageApplicationBuilder.Build(option);
        }

        [PlatformRestrictedFact(linux: true)]
        public async Task Build_OnLinux_No_Duplicate_Extension_Success()
        {
            using var packageApplicationBuilder = GetPackageApplicationBuilder();
            var option = new PackageOption
            {
                Name = "testapp.exe",
                Command = "python3",
                Version = "3.0.1.0",
                Include = new string[] { @"assets/include" },
                TargetOs = AppHostService.LINUX_RID
            };
            await packageApplicationBuilder.Build(option);
        }

        [PlatformRestrictedFact(windows: true)]
        public async Task Build_OnWindows_With_No_Duplicate_Exe_Extension_Success()
        {
            using var packageApplicationBuilder = GetPackageApplicationBuilder();
            var option = new PackageOption
            {
                Name = "testapp.exe",
                Command = "Notepad",
                Version = "3.0.1.0",
                Include = new string[] { @"assets\include" },
                TargetOs = AppHostService.WIN_RID
            };
            await packageApplicationBuilder.Build(option);
        }

        [Fact]
        public async Task Build_InvalidRID_ArgumentException()
        {
            using var packageApplicationBuilder = GetPackageApplicationBuilder();
            var option = new PackageOption
            {
                Name = "testapp.exe",
                Command = "Notepad",
                Version = "3.0.1.0",
                Include = new[] { @".\assets\" },
                TargetOs = "mac-os"
            };
            await Assert.ThrowsAsync<ArgumentException>(async () => await packageApplicationBuilder.Build(option));
        }

        [PlatformRestrictedFact(linux: true)]
        public async Task Build_TargetLinux_UnsupportedVersionAndIcon_ArgumentException()
        {
            var option = new PackageOption {
                Name = "testapp",
                Command = "Python3",
                Version = "2.0.0",
                Icon = @".\assets\TestIcon.ico"
            };
            var fileSystem = new MockFileSystem();
            var resource = new Mock<IResourcesAssemblyBuilder>();
            var appsettingsFile = new Mock<ILauncherAppsettingsFileService>();
            var appHostService = new Mock<IAppHostService>();
            var logger = new Mock<ILogger<PackageApplicationBuilder>>();
            var hostOsService = new Mock<IHostOsDetectionService>();
            hostOsService.Setup(x => x.GetTargetOsRid()).Returns(AppHostService.LINUX_RID)
                .Verifiable();
            hostOsService.Setup(x => x.IsVersionIconSupported(It.IsAny<PackageOption>())).Returns(false)
               .Verifiable();

            using var packageApplicationBuilder = new PackageApplicationBuilder(fileSystem, resource.Object, appsettingsFile.Object, appHostService.Object, logger.Object, hostOsService.Object);

            await Assert.ThrowsAsync<ArgumentException>(async () => await packageApplicationBuilder.Build(option));
            hostOsService.Verify();
        }

        [PlatformRestrictedFact(windows: true)]
        public async Task Build_TargetWindows_UnsupportedVersionAndIcon_ArgumentException()
        {
            var option = new PackageOption {
                Name = "testapp",
                Command = "Python3",
                Version = "2.0.0",
                Icon = @"./assets/TestIcon.ico"
            };
            var fileSystem = new MockFileSystem();
            var resource = new Mock<IResourcesAssemblyBuilder>();
            var appsettingsFile = new Mock<ILauncherAppsettingsFileService>();
            var appHostService = new Mock<IAppHostService>();
            var logger = new Mock<ILogger<PackageApplicationBuilder>>();
            var hostOsService = new Mock<IHostOsDetectionService>();
            hostOsService.Setup(x => x.GetTargetOsRid()).Returns(AppHostService.WIN_RID)
                .Verifiable();
            hostOsService.Setup(x => x.IsVersionIconSupported(It.IsAny<PackageOption>())).Returns(false)
               .Verifiable();

            using var packageApplicationBuilder = new PackageApplicationBuilder(fileSystem, resource.Object, appsettingsFile.Object, appHostService.Object, logger.Object, hostOsService.Object);

            await Assert.ThrowsAsync<ArgumentException>(async () => await packageApplicationBuilder.Build(option));
            hostOsService.Verify();
        }

    }
}
