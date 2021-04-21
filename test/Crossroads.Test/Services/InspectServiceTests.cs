using Crossroads.Core;
using Moq;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xunit;

namespace Crossroads.Services.Test
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
        [InlineData(@".\Crossroads.Launcher\Crossroads.Launcher.exe")]
        public async Task Inspect_Success(string packagePath)
        {
            var processService = new Mock<IProcessService>();
            processService.Setup(x => x.GetConsoleOutputAsync(It.Is<ProcessStartInfo>(x => x.Arguments == "inspect"), It.IsAny<int>()))
                .ReturnsAsync("Crossroads Inspect")
                .Verifiable();

            IInspectService inspectService = new InspectService(processService.Object);
            var actual = await inspectService.InspectLauncherPackage(packagePath);
            Assert.Contains("Crossroads Inspect", actual);

            processService.Verify();
        }
    }
}
