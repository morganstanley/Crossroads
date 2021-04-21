using Crossroads.Core;
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
