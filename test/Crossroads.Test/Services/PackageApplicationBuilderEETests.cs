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
using System;
using System.IO;
using System.IO.Abstractions;
using System.Threading.Tasks;
using Xunit;

namespace Crossroads.Test.Services
{
    public class PackageApplicationBuilderEETests
    {
        [PlatformRestrictedFact(windows: true)]
        public async Task Build_OnWindows_Success()
        {
            using var packageApplicationBuilder = GetPackageApplicationBuilder();
            var option = new DefaultOption
            {
                Command = "Notepad",
                Version = "3.0.1.0",
                TargetOs = AppHostService.WIN_RID
            };
            await packageApplicationBuilder.Build(option);
        }

        [PlatformRestrictedFact(linux: true)]
        public async Task Build_OnLinux_Success()
        {
            using var packageApplicationBuilder = GetPackageApplicationBuilder();
            var option = new DefaultOption
            {
                Command = "python3",
                TargetOs = AppHostService.LINUX_RID
            };
            await packageApplicationBuilder.Build(option);
        }

        [PlatformRestrictedFact(linux: true)]
        public async Task Build_OnLinux_WithVersion_Throws_ArgumentException()
        {
            using var packageApplicationBuilder = GetPackageApplicationBuilder();
            var expectedMessage = "Version or Icon is not required.";
            var option = new DefaultOption
            {
                Command = "python3",
                Version = "1.0.0.0",
                TargetOs = AppHostService.WIN_RID
            };
            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await packageApplicationBuilder.Build(option));
            Assert.Equal(expectedMessage, ex.Message);
        }

        [PlatformRestrictedFact(windows: true)]
        public async Task Build_OnWindows_Include_Success()
        {
            using var packageApplicationBuilder = GetPackageApplicationBuilder();
            var option = new DefaultOption
            {
                Command = "Notepad",
                Version = "3.0.1.0",
                Include = new[] { @".\assets\" },
                TargetOs = AppHostService.WIN_RID
            };
            await packageApplicationBuilder.Build(option);
        }

        [PlatformRestrictedFact(linux:true)]
        public async Task Build_OnLinux_Include_Success()
        {
            using var packageApplicationBuilder = GetPackageApplicationBuilder();
            var option = new DefaultOption
            {
                Command = "python3",
                Include = new[] { @"./assets" },
                TargetOs = AppHostService.LINUX_RID
            };
            await packageApplicationBuilder.Build(option);
        }

        [Fact]
        public async Task Build_InvalidRID_ArgumentException()
        {
            using var packageApplicationBuilder = GetPackageApplicationBuilder();
            var option = new DefaultOption
            {
                Command = "Notepad",
                Version = "3.0.1.0",
                Include = new[] { @".\assets\" },
                TargetOs = "invalid-RID"
            };
            await Assert.ThrowsAsync<ArgumentException>(async () => await packageApplicationBuilder.Build(option));
        }

        [Fact]
        public async Task Build_InvalidInclude_ArgumentException()
        {
            using var packageApplicationBuilder = GetPackageApplicationBuilder();
            var option = new DefaultOption
            {
                Command = "Notepad",
                Version = "3.0.1.0",
                Include = new[] { @".\assets\invalidinclude" },
                TargetOs = AppHostService.WIN_RID
            };
            await Assert.ThrowsAsync<ArgumentException>(async () => await packageApplicationBuilder.Build(option));
        }

        [PlatformRestrictedFact(windows: true)]
        public async Task Build_Icon_OnWindows_Success()
        {
            using var packageApplicationBuilder = GetPackageApplicationBuilder();
            var option = new DefaultOption
            {
                Icon = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "assets", "TestIcon.ico"),
                TargetOs = AppHostService.WIN_RID
            };
            await packageApplicationBuilder.Build(option);
        }

        [PlatformRestrictedFact(linux: true)]
        public async Task Build_WithIcon_OnLinux_Success()
        {
            using var packageApplicationBuilder = GetPackageApplicationBuilder();
            var expectedMessage = "Version or Icon is not required.";

            var option = new DefaultOption
            {
                Icon = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "assets", "TestIcon.ico"),
                TargetOs = AppHostService.WIN_RID
            };
            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await packageApplicationBuilder.Build(option));
            Assert.Equal(expectedMessage, ex.Message);
        }

        [Fact]
        public async Task Build_NoVersion_Success()
        {
            using var packageApplicationBuilder = GetPackageApplicationBuilder();
            var option = new DefaultOption
            {
                Command = "Notepad",
                TargetOs = AppHostService.WIN_RID
            };
            await packageApplicationBuilder.Build(option);
        }

        [Fact]
        public async Task Build_DetectHostOS_Success()
        {
            using var packageApplicationBuilder = GetPackageApplicationBuilder();
            var option = new DefaultOption
            {
                Command = "Notepad",
                TargetOs = null
            };
            await packageApplicationBuilder.Build(option);
        }

        private PackageApplicationBuilder GetPackageApplicationBuilder()
        {
            var fileSystem = new FileSystem();
            var resource = new ResourcesAssemblyBuilder(fileSystem);
            ILauncherAppsettingsFileService launcherAppsettingsFileService = new LauncherAppsettingsFileService(fileSystem);
            IAppHostService appHostService = new AppHostService();
            ILoggerFactory loggerFactory = new LoggerFactory();
            ILogger<PackageApplicationBuilder> logger = loggerFactory.CreateLogger<PackageApplicationBuilder>();
            IHostOsDetectionService hostOsDetectionService = new HostOsDetectionService();

            return new PackageApplicationBuilder(fileSystem, resource, launcherAppsettingsFileService, appHostService, logger, hostOsDetectionService);
        }

        private class DefaultOption : PackageOption
        {
            public DefaultOption()
            {
                Name = Path.ChangeExtension(Path.GetRandomFileName(), "exe");
                Location = Path.Combine(Path.GetTempPath(), "crossroads", Path.GetRandomFileName());
            }
        }
    }
}
