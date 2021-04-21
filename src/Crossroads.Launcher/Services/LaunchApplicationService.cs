using Crossroads.Core;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Abstractions;
using System.Threading.Tasks;

namespace Crossroads.Launcher.Services
{
    public class LaunchApplicationService : ILaunchApplicationService
    {
        private readonly PackageOption launcherOption;
        private readonly IFileSystem fileSystem;
        private readonly IProcessService processService;

        public LaunchApplicationService(IConfiguration configuration, IFileSystem fileSystem, IProcessService processService)
        {
            this.launcherOption = new PackageOption
            {
                Command = configuration["Launcher:Command"],
                Args = configuration["Launcher:Args"]
            };
            this.fileSystem = fileSystem;
            this.processService = processService;
        }

        public async Task<int> RunAsync(string arguments = null)
        {
            string command = launcherOption.Command;
            if (string.IsNullOrWhiteSpace(command))
            {
                throw new Exception("Command is not configured correctly.");
            }

            string workingDirectory;
            if (fileSystem.Directory.Exists(assetsDirectory))
            {
                string tmpCommand = Path.Combine(assetsDirectory, command);
                if (fileSystem.File.Exists(tmpCommand))
                {
                    command = tmpCommand;
                }
                workingDirectory = assetsDirectory;
            }
            else
            {
                workingDirectory = null;
            }

            var startInfo = new ProcessStartInfo
            {
                FileName = command,
                Arguments = string.IsNullOrWhiteSpace(arguments) ? launcherOption.Args : arguments,
                UseShellExecute = false,
                WorkingDirectory = workingDirectory
            };
            return await processService.RunAsync(startInfo);

        }

        private string assetsDirectory => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "assets");
    }
}
