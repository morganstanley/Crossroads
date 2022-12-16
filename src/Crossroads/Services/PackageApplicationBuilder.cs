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

using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Crossroads.Services
{
    public class PackageApplicationBuilder : IPackageApplicationBuilder, IDisposable
    {
        private readonly IFileSystem fileSystem;
        private readonly IResourcesAssemblyBuilder resourcesAssemblyBuilder;
        private readonly ILauncherAppsettingsFileService launcherAppsettingsFileService;
        private readonly IAppHostService appHostService;
        private readonly IHostOsDetectionService hostOsDetectionService;
        private readonly ILogger<PackageApplicationBuilder> _logger;

        public PackageApplicationBuilder(
            IFileSystem fileSystem, 
            IResourcesAssemblyBuilder resourcesAssemblyBuilder, 
            ILauncherAppsettingsFileService launcherAppsettingsFileService, 
            IAppHostService appHostService, 
            ILogger<PackageApplicationBuilder> logger, 
            IHostOsDetectionService hostOsDetectionService)
        {
            this.fileSystem = fileSystem;
            this.resourcesAssemblyBuilder = resourcesAssemblyBuilder;
            this.launcherAppsettingsFileService = launcherAppsettingsFileService;
            this.appHostService = appHostService;
            _logger = logger;
            this.hostOsDetectionService = hostOsDetectionService;
        }

        private bool disposed = false;

        public PackageOption Option { get; set; }

        public async Task Build(PackageOption option)
        {
            Option = option;
            if (Option == null)
            {
                throw new ArgumentException(nameof(Option));
            }
            if (string.IsNullOrWhiteSpace(Option.Name))
            {
                throw new ArgumentException(nameof(Option.Name));
            }
            if (!hostOsDetectionService.IsVersionIconSupported(option))
            {
                throw new ArgumentException($"{nameof(Option.Version)} or {nameof(Option.Icon)} not required.");
            }
            if (String.IsNullOrEmpty(option.TargetOs))
            {
                option.TargetOs = hostOsDetectionService.GetTargetOsRid();
            }
            if (!(option.TargetOs.Equals(AppHostService.WIN_RID) || option.TargetOs.Equals(AppHostService.LINUX_RID)))
            {
                throw new ArgumentException($"Invalid RID: {option.TargetOs}");
            }

            await Task.Run(() => CopyDirectory(launcherSourceDirectory, appHostDirectory, true));

            await launcherAppsettingsFileService.SetOption(appSettingsFilePath, option);

            CopyIncludeDirectories();

            string resourceassemblyPathResult = await resourcesAssemblyBuilder.Build(resourceassemblyPath, Option.Version, Option.Icon);
            string fileName = (option.TargetOs == AppHostService.LINUX_RID) ? Path.GetFileNameWithoutExtension(Option.Name) :
                (string.Compare(Path.GetExtension(Option.Name), ".exe", true) == 0) ? Option.Name : $"{Option.Name}.exe";
            await appHostService.ConvertLauncherToBundle(fileName, Option.Location, appHostDirectory, resourceassemblyPathResult, Option.TargetOs);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }
            if (disposing)
            {
                cleanWorkingDirectory();
            }
            disposed = true;
        }

        private void cleanWorkingDirectory()
        {
            try
            {
                if (fileSystem.Directory.Exists(workingDirectory))
                {
                    fileSystem.Directory.Delete(workingDirectory, true);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Optionally delete file in working directory", ex.Message);
            }
        }

        private string WorkingDirectory
        {
            get
            {
                if (string.IsNullOrWhiteSpace(workingDirectory))
                {
                    workingDirectory = Path.Combine(Path.GetTempPath(), "crossroads", Path.GetRandomFileName());
                    if (!fileSystem.Directory.Exists(workingDirectory))
                    {
                        fileSystem.Directory.CreateDirectory(workingDirectory);
                    }
                }
                return workingDirectory;
            }
        }

        private void CopyIncludeDirectories()
        {
            if (Option.Include == null)
            {
                return;
            }

            var invalidIncludes = Option.Include.Where(x => !fileSystem.Directory.Exists(x)).ToArray();
            if (invalidIncludes.Length == 1)
            {
                throw new ArgumentException($"Invalid include directory {invalidIncludes.First()}");
            }
            else if (invalidIncludes.Length > 1)
            {
                throw new AggregateException("Invalid include directories", invalidIncludes.Select(x => new ArgumentException($"Invalid include directory {x}")));
            }

            foreach (var includeDirectory in Option.Include)
            {
                IDirectoryInfo dirInfo = fileSystem.DirectoryInfo.FromDirectoryName(includeDirectory);
                CopyDirectory(includeDirectory, Path.Combine(assetsDirectory, dirInfo.Name), true);
            }
        }

        private string workingDirectory;

        private string appHostDirectory => Path.Combine(WorkingDirectory, "AppDirectory");
        private string launcherSourceDirectory
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Crossroads.Launcher", Option.TargetOs);
            }
        }

        private string appSettingsFilePath => Path.Combine(appHostDirectory, "appsettings.json");

        private string resourceassemblyPath => Path.Combine(appHostDirectory, "crossroads.resourceassembly.dll");
        private string assetsDirectory => Path.Combine(appHostDirectory, "assets");

        private void CopyDirectory(string sourceDirName, string destDirName, bool copySubDirs)
        {
            IDirectoryInfo dir = fileSystem.DirectoryInfo.FromDirectoryName(sourceDirName);

            IDirectoryInfo[] dirs = dir.GetDirectories();
                                        
            fileSystem.Directory.CreateDirectory(destDirName);

            IFileInfo[] files = dir.GetFiles();
            foreach (FileInfoBase file in files)
            {
                string tempPath = Path.Combine(destDirName, file.Name);
                file.CopyTo(tempPath, false);
            }

            if (copySubDirs)
            {
                foreach (DirectoryInfoBase subdir in dirs)
                {
                    string tempPath = Path.Combine(destDirName, subdir.Name);
                    CopyDirectory(subdir.FullName, tempPath, copySubDirs);
                }
            }
        }
    }
}
