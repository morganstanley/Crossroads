﻿/*
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
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
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

        [Fact]
        public async Task Build_InvalidInclude_ArgumentException()
        {
            using var packageApplicationBuilder = GetPackageApplicationBuilder();
            var option = new PackageOption
            {
                Name = "testapp",
                Command = "Notepad",
                Version = "3.0.1.0",
                Include = new[] { @".\assets\invalidinclude" }
            };
            await Assert.ThrowsAsync<ArgumentException>(async () => await packageApplicationBuilder.Build(option));
        }

        [Fact]
        public async Task Build_InvalidIncludes_ArgumentException()
        {
            using var packageApplicationBuilder = GetPackageApplicationBuilder();
            var option = new PackageOption
            {
                Name = "testapp",
                Command = "Notepad",
                Version = "3.0.1.0",
                Include = new[] { @".\assets\invalidinclude", @".\assets\invalidinclude2" }
            };
            await Assert.ThrowsAsync<AggregateException>(async () => await packageApplicationBuilder.Build(option));
        }

        [Fact]
        public async Task Build_OptionNull_Argumentexception()
        {
            using var packageApplicationBuilder = GetPackageApplicationBuilder();
            await Assert.ThrowsAsync<ArgumentException>(async () => await packageApplicationBuilder.Build(null));
        }

        [Fact]
        public async Task Build_Success()
        {
            using var packageApplicationBuilder = GetPackageApplicationBuilder();
            var option = new PackageOption
            {
                Name = "testapp",
                Command = "Notepad",
                Version = "3.0.1.0",
                Include = new string[] { @"assets\include" }
            };
            await packageApplicationBuilder.Build(option);
        }

        [Fact]
        public async Task Build_Success_Exception()
        {
            var fileSystem = new Mock<IFileSystem>();
            fileSystem.Setup(x => x.DirectoryInfo.FromDirectoryName(It.IsAny<string>())).Returns(It.IsAny<DirectoryInfoBase>());
            fileSystem.Setup(x => x.DirectoryInfo.FromDirectoryName(It.IsAny<string>()).GetDirectories()).Returns(It.IsAny<DirectoryInfoBase[]>());
            fileSystem.Setup(x => x.DirectoryInfo.FromDirectoryName(It.IsAny<string>()).GetFiles()).Returns(Array.Empty<FileInfoBase>);
            fileSystem.Setup(x => x.DirectoryInfo.FromDirectoryName(It.IsAny<string>()).GetDirectories()).Returns(Array.Empty<DirectoryInfoBase>);
            fileSystem.Setup(x => x.Directory.Exists(It.IsAny<string>())).Returns(true);
            fileSystem.Setup(x => x.Directory.Delete(It.IsAny<string>(), It.IsAny<bool>())).Throws(new Exception());
            var resource = new Mock<IResourcesAssemblyBuilder>();
            var appsettingsFile = new Mock<ILauncherAppsettingsFileService>();
            var appHostService = new Mock<IAppHostService>();
            var logger = new Mock<ILogger<PackageApplicationBuilder>>();
            var packageApp = new PackageApplicationBuilder(fileSystem.Object, resource.Object, appsettingsFile.Object, appHostService.Object, logger.Object);

            var option = new PackageOption
            {
                Name = "testapp",
                Command = "Notepad",
                Version = "3.0.1.0",
                Include = null,
            };

            await packageApp.Build(option);
            packageApp.Dispose();
        }

        private PackageApplicationBuilder GetPackageApplicationBuilder()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.AddFile(@"assets\include\include2\file1.txt", new MockFileData("abc"));
            fileSystem.AddDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Crossroads.Launcher", "linux-x64"));
            var resource = new Mock<IResourcesAssemblyBuilder>();
            var appsettingsFile = new Mock<ILauncherAppsettingsFileService>();
            var appHostService = new Mock<IAppHostService>();
            var logger = new Mock<ILogger<PackageApplicationBuilder>>();

            return new PackageApplicationBuilder(fileSystem, resource.Object, appsettingsFile.Object, appHostService.Object, logger.Object);
        }

    }
}
