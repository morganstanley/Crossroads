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
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Crossroads.Test.Services
{
    public class ProcessServiceTests
    {
        [Fact]
        public async Task Run_Cmd_Success()
        {
            IProcessService processService = new ProcessService();
            var startInfo = new ProcessStartInfo
            {
                FileName = "cmd",
                Arguments = "/c echo hello"
            };
            var actual = await processService.RunAsync(startInfo);
            Assert.Equal(0, actual);
        }

        [Fact]
        public async Task Run_BadCmd_Exception()
        {
            IProcessService processService = new ProcessService();
            var startInfo = new ProcessStartInfo
            {
                FileName = "bad"
            };
            await Assert.ThrowsAnyAsync<Exception>(async () => await processService.RunAsync(startInfo));

        }

        [Fact]
        public async Task GetConsoleOutput_Success()
        {
            IProcessService processService = new ProcessService();
            var startInfo = new ProcessStartInfo
            {
                FileName = "cmd",
                Arguments = "/c echo hello"
            };
            var actual = await processService.GetConsoleOutputAsync(startInfo, 3000);
            Assert.Contains("hello", actual);
        }

        
        [Fact]
        public async Task GetConsoleOutput_Output_TimeoutException()
        {
            IProcessService processService = new ProcessService();
            var startInfo = new ProcessStartInfo
            {
                FileName = "cmd",
                Arguments = "/k echo wait time out"
            };
            await Assert.ThrowsAsync<TimeoutException>(async () => await processService.GetConsoleOutputAsync(startInfo, 3000));
        }

        [Fact]
        public async Task GetConsoleOutput_Return1_TimeoutException()
        {
            IProcessService processService = new ProcessService();
            var startInfo = new ProcessStartInfo
            {
                FileName = "cmd",
                Arguments = "/c exit 1"
            };
            await Assert.ThrowsAsync<Exception>(async () => await processService.GetConsoleOutputAsync(startInfo, 3000));
        }
    }
}
