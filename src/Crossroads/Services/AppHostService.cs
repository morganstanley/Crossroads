using Microsoft.NET.HostModel.AppHost;
using Microsoft.NET.HostModel.Bundle;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Crossroads.Services
{
    public class AppHostService : IAppHostService
    {
        public async Task ConvertLauncherToBundle(string bundleName, string bundleDirectory, string appHostDirectory, string resourceassemblyPathResult)
        {
            var appHostDestinationFilePath = Path.Combine(appHostDirectory, bundleName);
            await Task.Run(() => HostWriter.CreateAppHost(appHostSourceFilePath, appHostDestinationFilePath, appBinaryFilePath, assemblyToCopyResorcesFrom: resourceassemblyPathResult));

            var bundler = new Bundler(bundleName, bundleDirectory);
            await Task.Run(() => bundler.GenerateBundle(appHostDirectory));
        }
        private string appHostSourceFilePath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AppHost", "apphost.exe");
        private string appBinaryFilePath => "CrossRoads.Launcher.dll";

    }
}
