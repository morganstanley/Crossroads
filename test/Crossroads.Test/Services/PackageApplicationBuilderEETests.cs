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
using System;
using System.IO;
using System.IO.Abstractions;
using System.Threading.Tasks;
using Xunit;

namespace Crossroads.Test.Services
{
    public class PackageApplicationBuilderEETests
    {
        [Fact]
        public async Task Build_Success()
        {
            using var packageApplicationBuilder = GetPackageApplicationBuilder();
            var option = new DefaultOption
            {
                Command = "Notepad",
                Version = "3.0.1.0"
            };
            await packageApplicationBuilder.Build(option);
        }

        [Fact]
        public async Task Build_Include_Success()
        {
            using var packageApplicationBuilder = GetPackageApplicationBuilder();
            var option = new DefaultOption
            {
                Command = "Notepad",
                Version = "3.0.1.0",
                Include = new[] { @".\assets\include1" }
            };
            await packageApplicationBuilder.Build(option);
        }

        [Fact]
        public async Task Build_InvalidInclude_ArgumentException()
        {
            using var packageApplicationBuilder = GetPackageApplicationBuilder();
            var option = new DefaultOption
            {
                Command = "Notepad",
                Version = "3.0.1.0",
                Include = new[] { @".\assets\invalidinclude" }
            };
            await Assert.ThrowsAsync<ArgumentException>(async () => await packageApplicationBuilder.Build(option));
        }

        [Fact]
        public async Task Build_Icon_Success()
        {
            using var packageApplicationBuilder = GetPackageApplicationBuilder();
            var option = new DefaultOption
            {
                Icon = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "assets", "TestIcon.ico")
            };
            await packageApplicationBuilder.Build(option);
        }

        [Fact]
        public async Task Build_NoVersion_Success()
        {
            using var packageApplicationBuilder = GetPackageApplicationBuilder();
            var option = new DefaultOption
            {
                Command = "Notepad"
            };
            await packageApplicationBuilder.Build(option);
        }

        private PackageApplicationBuilder GetPackageApplicationBuilder()
        {
            var fileSystem = new FileSystem();
            var resource = new ResourcesAssemblyBuilder(fileSystem);
            ILauncherAppsettingsFileService launcherAppsettingsFileService = new LauncherAppsettingsFileService(fileSystem);
            IAppHostService appHostService = new AppHostService();
            return new PackageApplicationBuilder(fileSystem, resource, launcherAppsettingsFileService, appHostService);
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
