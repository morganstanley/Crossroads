using Crossroads.Launcher.Services;
using Crossroads.Test.Utility;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Crossroads.Launcher.Commands.Test
{
    public class LauncherRootCommandTests
    {
        [Fact]
        public async Task RootCommand_Run_Success()
        {
            var launchApp = new Mock<ILaunchApplicationService>();
            launchApp.Setup(x => x.RunAsync(It.IsAny<string>()))
                .ReturnsAsync(0)
                .Verifiable();
            var command = new LauncherRootCommand();
            var actual = await command.ExecuteSystemCommand(null, (_, services) =>
            {
                services.AddSingleton<ILaunchApplicationService>(launchApp.Object);
            });

            Assert.Equal(0, actual);
            launchApp.Verify();
        }

        [Fact]
        public async Task RootCommand_Run_WhenArgs_Success()
        {
            var launchApp = new Mock<ILaunchApplicationService>();
            launchApp.Setup(x => x.RunAsync(It.IsAny<string>()))
                .ReturnsAsync(0)
                .Verifiable();
            var command = new LauncherRootCommand();
            var actual = await command.ExecuteSystemCommand("/c echo withargs", (_, services) =>
            {
                services.AddSingleton<ILaunchApplicationService>(launchApp.Object);
            });

            Assert.Equal(0, actual);
            launchApp.Verify();
        }

        [Fact]
        public async Task RootCommand_Run_FailedLaunch_Exception()
        {
            var launchApp = new Mock<ILaunchApplicationService>();
            launchApp.Setup(x => x.RunAsync(It.IsAny<string>()))
                .ThrowsAsync(Mock.Of<Exception>())
                .Verifiable();
            var command = new LauncherRootCommand();
            var actual = await command.ExecuteSystemCommand(null, (_, services) =>
            {
                services.AddSingleton<ILaunchApplicationService>(launchApp.Object);
            });

            Assert.Equal(1, actual);
            launchApp.Verify();
        }
    }
}
