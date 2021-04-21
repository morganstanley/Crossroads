using Crossroads.Core;
using Crossroads.Services;
using Crossroads.Test.Utility;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Crossroads.Commands.Test
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
