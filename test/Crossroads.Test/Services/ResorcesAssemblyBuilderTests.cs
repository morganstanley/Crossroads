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
using System.IO.Abstractions.TestingHelpers;
using System.Threading.Tasks;
using Xunit;

namespace Crossroads.Test.Services
{
    public class ResorcesAssemblyBuilderTests
    {
        private string iconPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "assets", "TestIcon.ico");

        [Theory]
        [InlineData("abc", "2.1.0.0", "good")]
        [InlineData("abc", null, "good")]
        [InlineData(null, null, null)]
        [InlineData("abc", "2.1.0.0", null)]
        [InlineData("abc", "1.2", null)]
        public async Task Build_Success(string targetPath, string version, string iconPath)
        {
            IResourcesAssemblyBuilder builder = GetResourcesAssemblyBuilder();
            var actual = await builder.Build(targetPath, version, iconPath);
            Assert.Equal(targetPath, actual);
        }

        private IResourcesAssemblyBuilder GetResourcesAssemblyBuilder()
        {
            var fileSystem = new MockFileSystem();
            var iconBytes = File.ReadAllBytes(iconPath);
            var iconFile = new MockFileData(iconBytes);
            fileSystem.AddFile("good", iconFile);

            return new ResourcesAssemblyBuilder(fileSystem);
        }

        [Theory]
        [InlineData("abc", null, "bad")]
        [InlineData("abc", "bad", null)]
        public async Task Build_ArgsInvalid_ArgumentException(string targetPath, string version, string iconPath)
        {
            IResourcesAssemblyBuilder builder = GetResourcesAssemblyBuilder();
            await Assert.ThrowsAsync<ArgumentException>(async () => await builder.Build(targetPath, version, iconPath));
        }

        [Fact]
        public async Task Build_VersionInvalid_BuildException()
        {
            IResourcesAssemblyBuilder builder = GetResourcesAssemblyBuilder();
            await Assert.ThrowsAsync<Exception>(async () => await builder.Build("abc", @"1.""2", null));
        }
    }
}
