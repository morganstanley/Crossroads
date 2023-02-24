using Crossroads.Launcher.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace Crossroads.Launcher.Test.Services
{
    public class LauncherInspectServiceTests
    {
        [Fact]
        public void DisplayOption_Success()
        {
            IConfiguration configuration = Mock.Of<IConfiguration>(x => x["Launcher:Name"] == "testapp"
                && x["Launcher:Location"] == "");
            ILauncherInspectService service = new LauncherInspectService(configuration);

            service.DisplayOption();
        }

        [Fact]
        public void DisplayOption_WithInclude_Success()
        {
            IConfiguration configuration = Mock.Of<IConfiguration>(x => 
                x["Launcher:Name"] == "testapp" && 
                x["Launcher:Location"] == "" &&
                x["Launcher:Include"] == "include/path/dir"
            );
            ILauncherInspectService service = new LauncherInspectService(configuration);

            service.DisplayOption();
        }

        [Fact]
        public void DisplayOption_WithMultipleIncludes_Success()
        {
            var configurationMock = new Mock<IConfiguration>();
            var include1SectionMock = new Mock<IConfigurationSection>();
            var include2SectionMock = new Mock<IConfigurationSection>();
            var includeSectionMock = new Mock<IConfigurationSection>();

            include1SectionMock.Setup(s => s.Value).Returns("include1");
            include2SectionMock.Setup(s => s.Value).Returns("include2");
            includeSectionMock.Setup(s => s.GetChildren()).Returns(new List<IConfigurationSection> { include1SectionMock.Object, include2SectionMock.Object });
            configurationMock.Setup(x => x.GetSection("Launcher:Include")).Returns(includeSectionMock.Object);

            ILauncherInspectService service = new LauncherInspectService(configurationMock.Object);
            service.DisplayOption();
            configurationMock.Verify(c => c.GetSection("Launcher:Include"), Times.Once);
        }

        [Fact]
        public void Display_Option_With_TargetOS_Success()
        {
            IConfiguration configuration = Mock.Of<IConfiguration>(x =>
                x["Launcher:Name"] == "testapp" &&
                x["Launcher:TargetOs"] == "win-x64" &&
                x["Launcher:Include"] == "include/path/dir" && 
                x["Launcher:Location"] == "" 
            );
            ILauncherInspectService service = new LauncherInspectService(configuration);

            service.DisplayOption();
        }


    }
}
