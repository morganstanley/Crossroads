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
using Crossroads.Test.Utility;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xunit;

namespace Crossroads.Test.Services
{
    public class ProcessServiceTests
    {
        [PlatformRestrictedFact(windows: true)]
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

        [PlatformRestrictedFact(windows: true)]
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

        [PlatformRestrictedFact(windows: true)]
        public async Task GetConsoleOutput_Output_TimeoutException()
        {
            IProcessService processService = new ProcessService();
            var startInfo = new ProcessStartInfo
            {
                FileName = "cmd",
                Arguments = "Timeout /t 50 /nobreak"
            };
            try
            {
                var actual = await processService.GetConsoleOutputAsync(startInfo, 3000);
            }
            catch (TimeoutException ex)
            {
                Assert.Equal("Package: cmd", ex.Message);
            }
        }

        [PlatformRestrictedFact(windows: true)]
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
