using Crossroads.Commands;
using Crossroads.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using System;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Parsing;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Crossroads.Commands.Test
{
    public class InspectCommandTests
    {
        [Fact]
        public async Task Inspect_Success()
        {
            StringWriter consoleOutWriter = new StringWriter();
            Console.SetOut(consoleOutWriter);

            string expected = "Good Console Output";
            var inspectService = new Mock<IInspectService>();
            inspectService.Setup(service => service.InspectLauncherPackage(It.IsAny<string>()))
                .ReturnsAsync(expected)
                .Verifiable();
            var parser = new CommandLineBuilder(new InspectCommand())
                .UseHost(_ => Host.CreateDefaultBuilder(),
                hostBuilder =>
                {
                    hostBuilder.ConfigureServices(services =>
                    {
                        services.AddSingleton<IInspectService>(inspectService.Object);
                    });
                }).Build();

            var parseResult = parser.Parse("inspect --package goodpackage");
            var exitCode = await parseResult.InvokeAsync();
            var actual = consoleOutWriter.ToString();
            Assert.Contains(expected, actual);
            inspectService.Verify();
        }
    }
}
