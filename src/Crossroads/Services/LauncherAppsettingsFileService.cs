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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO.Abstractions;
using System.Threading.Tasks;

namespace Crossroads.Services
{
    public class LauncherAppsettingsFileService : ILauncherAppsettingsFileService
    {
        private readonly IFileSystem fileSystem;

        public LauncherAppsettingsFileService(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }
        public async Task SetOption(string filePath, PackageOption option)
        {
            await Task.Run(() =>
            {
                string json = fileSystem.File.ReadAllText(filePath);
                dynamic jsonObj = JsonConvert.DeserializeObject(json);
                jsonObj["Launcher"]["Name"] = option.Name;
                jsonObj["Launcher"]["Version"] = option.Version;
                jsonObj["Launcher"]["Icon"] = option.Icon;
                jsonObj["Launcher"]["Include"] = option.Include == null ? null : JToken.FromObject(option.Include);
                jsonObj["Launcher"]["Command"] = option.Command;
                jsonObj["Launcher"]["Args"] = option.Args;
                jsonObj["Launcher"]["Location"] = option.Location;
                string output = JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
                fileSystem.File.WriteAllText(filePath, output);
            });
        }
    }
}
