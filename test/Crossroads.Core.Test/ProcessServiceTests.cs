using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Crossroads.Core.Test
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
            var actual  = await processService.RunAsync(startInfo);
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
