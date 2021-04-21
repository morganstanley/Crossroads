using Crossroads.Services;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Crossroads.Services.Test
{
    public class AppHostServiceTests
    {
        [Fact]
        public async Task Convert_NoResource_Success()
        {
            string bundleName = Path.GetRandomFileName();
            string bundleDirectory = Path.Combine(Path.GetTempPath(), "crossroads", Path.GetRandomFileName());
            string appHostDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Crossroads.Launcher");

            IAppHostService appHost = new AppHostService();
            await appHost.ConvertLauncherToBundle(bundleName, bundleDirectory, appHostDirectory, null);
        }

        [Fact]
        public async Task Convert_WithResource_Success()
        {
            string bundleName = Path.GetRandomFileName();
            string bundleDirectory = Path.Combine(Path.GetTempPath(), "crossroads", Path.ChangeExtension(Path.GetRandomFileName(), "exe"));
            string appHostDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Crossroads.Launcher");

            IAppHostService appHost = new AppHostService();
            await appHost.ConvertLauncherToBundle(bundleName, bundleDirectory, appHostDirectory, Path.Combine(appHostDirectory, "CrossRoads.Launcher.dll"));
        }

    }
}
