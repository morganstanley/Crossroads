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

using System;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;

namespace Crossroads.Services
{
    public class PackageApplicationBuilder : IPackageApplicationBuilder, IDisposable
    {
        private readonly IFileSystem fileSystem;
        private readonly IResourcesAssemblyBuilder resourcesAssemblyBuilder;
        private readonly ILauncherAppsettingsFileService launcherAppsettingsFileService;
        private readonly IAppHostService appHostService;

        public PackageApplicationBuilder(IFileSystem fileSystem, IResourcesAssemblyBuilder resourcesAssemblyBuilder, ILauncherAppsettingsFileService launcherAppsettingsFileService, IAppHostService appHostService)
        {
            this.fileSystem = fileSystem;
            this.resourcesAssemblyBuilder = resourcesAssemblyBuilder;
            this.launcherAppsettingsFileService = launcherAppsettingsFileService;
            this.appHostService = appHostService;
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
            await Task.Run(() => CopyDirectory(launcherSourceDirectory, appHostDirectory, true));

            await launcherAppsettingsFileService.SetOption(appSettingsFilePath, option);

            // copy include files
            CopyIncludeDirectories();

            string resourceassemblyPathResult = await resourcesAssemblyBuilder.Build(resourceassemblyPath, Option.Version, Option.Icon);
            await appHostService.ConvertLauncherToBundle(Path.ChangeExtension(Option.Name, "exe"), Option.Location, appHostDirectory, resourceassemblyPathResult);
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
            if (fileSystem.Directory.Exists(workingDirectory))
            {
                fileSystem.Directory.Delete(workingDirectory, true);
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
                DirectoryInfoBase dirInfo = fileSystem.DirectoryInfo.FromDirectoryName(includeDirectory);
                CopyDirectory(includeDirectory, Path.Combine(assetsDirectory, dirInfo.Name), true);
            }
        }

        private string workingDirectory;
        //WorkingDirectory contains path to temp/crossroads/random/'AppDirectory'
        private string appHostDirectory => Path.Combine(WorkingDirectory, "AppDirectory");
        private string launcherSourceDirectory => Path.Combine(AppDomain.CurrentDomain.BaseDirectory);

        //appsettings creates new appsettings.json file in path to temp/crossroads/random/'AppDirectory'
        private string appSettingsFilePath => Path.Combine(appHostDirectory, "appsettings.json");

        //creates resource .dll inside temp
        private string resourceassemblyPath => Path.Combine(appHostDirectory, "crossroads.resourceassembly.dll");
        private string assetsDirectory => Path.Combine(appHostDirectory, "assets");


        private void CopyDirectory(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfoBase dir = fileSystem.DirectoryInfo.FromDirectoryName(sourceDirName);

            DirectoryInfoBase[] dirs = dir.GetDirectories();
            //FileInfoBase[] dirs = dir.GetFiles();
                                        
            // If the destination directory doesn't exist, create it.       
            fileSystem.Directory.CreateDirectory(destDirName);

            // Get the files in the directory and copy them to the new location.
            FileInfoBase[] files = dir.GetFiles();
            foreach (FileInfoBase file in files)
            {
                string tempPath = Path.Combine(destDirName, file.Name);
                file.CopyTo(tempPath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
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
