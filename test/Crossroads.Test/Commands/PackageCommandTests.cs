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
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Crossroads.Test.Commands
{
    public class PackageCommandTests
    {
        [Fact]
        public async Task Package_Success()
        {
            var packageApplication = new Mock<IPackageApplicationBuilder>();
            packageApplication.Setup(x => x.Build(It.IsAny<PackageOption>()))
                .Verifiable();

            var command = new PackageCommand();
            var actual = await command.ExecuteSystemCommand("package --name newnotepad --command notepad", (_, services) =>
            {
                services.AddSingleton<IPackageApplicationBuilder>(packageApplication.Object);
            });

            Assert.Equal(0, actual);
            packageApplication.Verify();
        }

        [Fact]
        public async Task Package_Exception()
        {
            var packageApplication = new Mock<IPackageApplicationBuilder>();
            packageApplication.Setup(x => x.Build(It.IsAny<PackageOption>()))
                .ThrowsAsync(new Exception("mock"))
                .Verifiable();

            var command = new PackageCommand();
            var actual = await command.ExecuteSystemCommand("package --name newnotepad --command notepad", (_, services) =>
            {
                services.AddSingleton<IPackageApplicationBuilder>(packageApplication.Object);
            });
            Assert.Equal(1, actual);
            packageApplication.Verify();
        }

    }
}
