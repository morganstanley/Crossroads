using Crossroads.Core;
using Crossroads.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Crossroads.Services.Test
{
    public class AppsettingsFileSystemTests
    {
        [Fact]
        public async Task Success()
        {
            MockFileSystem fileSystem = new MockFileSystem();
            fileSystem.AddFile("goodAppsettings", new MockFileData(@"{""Launcher"": {""Command"": """", ""Args"":  """" }}"));
            ILauncherAppsettingsFileService fileService = new LauncherAppsettingsFileService(fileSystem);
            await fileService.SetOption("goodAppsettings", 
                new PackageOption 
                {
                    Name = "nameAppSettings",
                    Include = new string[] {"include1", "include2"}
                });
        }
    }
}
