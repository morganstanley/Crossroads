using Crossroads.Launcher.Services;
using Microsoft.Extensions.Configuration;
using Moq;
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
    }
}
