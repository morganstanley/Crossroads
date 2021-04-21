using Crossroads.Launcher.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Crossroads.Launcher.Services.Test
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
    }
}
