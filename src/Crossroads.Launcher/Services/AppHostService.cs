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

        // path to bin win64
        private string appHostSourceFilePath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AppHost", "apphost.exe");
        private string appBinaryFilePath => "CrossRoads.dll";

    }
}
