/*
 * Morgan Stanley makes this available to you under the Apache License,
 * Version 2.0 (the "License"). You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0.
 *
 * See the NOTICE file distributed with this work for additional information
 * regarding copyright ownership. Unless required by applicable law or agreed
 * to in writing, software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
 * or implied. See the License for the specific language governing permissions
 * and limitations under the License.
 */

using Crossroads.Services;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Crossroads.Test.Services
{
    public class AppHostServiceTests
    {
        [Fact]
        public async Task Convert_NoResource_Success()
        {
            string bundleName = Path.GetRandomFileName();
            string bundleDirectory = Path.Combine(Path.GetTempPath(), "crossroads", $"{Path.GetRandomFileName()}.ext");
            string appHostDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Crossroads.Launcher");
            string rId = "win-x64";

            IAppHostService appHost = new AppHostService();
            await appHost.ConvertLauncherToBundle(bundleName, bundleDirectory, appHostDirectory, null, rId);
        }

        [Fact(Skip = "true")]
        public async Task Convert_WithResource_Success()
        {
            string bundleName = Path.GetRandomFileName();
            string bundleDirectory = Path.Combine(Path.GetTempPath(), "crossroads", Path.ChangeExtension(Path.GetRandomFileName(), "exe"));
            string appHostDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Crossroads.Launcher");
            string rId = "win-x64";

            IAppHostService appHost = new AppHostService();
            await appHost.ConvertLauncherToBundle(bundleName, bundleDirectory, appHostDirectory, rId, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CrossRoads.dll"));
        }

        [Fact]
        public async Task Convert_GetAppHostSourceFilePath_ApplicationException()
        {
            string bundleName = Path.GetRandomFileName();
            string bundleDirectory = Path.Combine(Path.GetTempPath(), "crossroads", $"{Path.GetRandomFileName()}.ext");
            string appHostDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Crossroads.Launcher", "invalidPath");
            string platformBundler = "win-x64";

            IAppHostService appHost = new AppHostService();
            await Assert.ThrowsAsync<ApplicationException>(async () => await appHost.ConvertLauncherToBundle(bundleName, bundleDirectory, appHostDirectory, platformBundler, null));
        }

        [Fact]
        public async Task Convert_GetAppHostSourceFilePath_Linux_Success()
        {
            string bundleName = Path.GetRandomFileName();
            string bundleDirectory = Path.Combine(Path.GetTempPath(), "crossroads", $"{Path.GetRandomFileName()}.ext");
            string appHostDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Crossroads.Launcher", "invalidPath");
            string platformBundler = "win-x64";

            IAppHostService appHost = new AppHostService();
            await Assert.ThrowsAsync<ApplicationException>(async () => await appHost.ConvertLauncherToBundle(bundleName, bundleDirectory, appHostDirectory, platformBundler, null));
        }
    }
}
