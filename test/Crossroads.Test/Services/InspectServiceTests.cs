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
using Moq;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xunit;

namespace Crossroads.Test.Services
{
    public class InspectServiceTests
    {
        [Theory]
        [InlineData("badPath")]
        [InlineData(@"C:\windows\win.ini")]
        public async Task Inspect_BadPackagePath_ErrorMessage(string packagePath)
        {
            var processService = new Mock<IProcessService>();
            processService.Setup(x => x.GetConsoleOutputAsync(It.IsAny<ProcessStartInfo>(), It.IsAny<int>()))
                .ThrowsAsync(new Exception())
                .Verifiable();
            IInspectService inspectService = new InspectService(processService.Object);
            var actual = await inspectService.InspectLauncherPackage(packagePath);
            Assert.Equal($"Package {packagePath} is not valid.", actual);
            processService.Verify();
        }


        [Theory]
        [InlineData(@".\Crossroads\Crossroads.exe")]
        public async Task Inspect_Success(string packagePath)
        {
            var processService = new Mock<IProcessService>();
            processService.Setup(x => x.GetConsoleOutputAsync(It.Is<ProcessStartInfo>(x => x.Arguments == "LauncherInspect"), It.IsAny<int>()))
                .ReturnsAsync("Crossroads Inspect")
                .Verifiable();

            IInspectService inspectService = new InspectService(processService.Object);
            var actual = await inspectService.InspectLauncherPackage(packagePath);
            Assert.Contains("Crossroads Inspect", actual);

            processService.Verify();
        }
    }
}
