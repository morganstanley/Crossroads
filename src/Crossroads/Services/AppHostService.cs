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
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Crossroads.Services
{
    public class AppHostService : IAppHostService
    {
        public async Task ConvertLauncherToBundle(string hostName, string outputDir, string appHostDirectory, string resourceassemblyPathResult, string rId)
        {
            var appHostDestinationFilePath = Path.Combine(appHostDirectory, hostName);
            await Task.Run(() => HostWriter.CreateAppHost(GetAppHostSourceFilePath(appHostDirectory, rId), appHostDestinationFilePath, appBinaryFilePath, assemblyToCopyResourcesFrom: resourceassemblyPathResult));
            
            var platformBundler = ((rId == "win-x64") ? OSPlatform.Windows : OSPlatform.Linux);
            var bundler = new Bundler(hostName, outputDir,
                BundleOptions.BundleAllContent | BundleOptions.BundleSymbolFiles,
                platformBundler,
                Architecture.X64,
                Version.Parse("6.0.10"),
                false,
                "Crossroads.Launcher",
                false);
            
            var fileSpecs = GenerateFileSpecs(appHostDirectory);
            await Task.Run(() => bundler.GenerateBundle(fileSpecs));
        }

        private List<FileSpec> GenerateFileSpecs(string sourceDir)
        {
            sourceDir = Path.GetFullPath(sourceDir);
            string[] files = Directory.GetFiles(sourceDir, "*", SearchOption.AllDirectories);
            Array.Sort(files, (IComparer<string>)StringComparer.Ordinal);
            List<FileSpec> fileList = new List<FileSpec>(files.Length);
            string[] array = files;
            foreach (string path in array)
            {
                fileList.Add(new FileSpec(path, RelativePath(sourceDir, path)));
            }
            return fileList;
        }

        private string RelativePath(string dirFullPath, string fileFullPath)
        {
            return fileFullPath.Substring(dirFullPath.TrimEnd(new char[1]
            {
                Path.DirectorySeparatorChar
            }).Length).TrimStart(new char[1]
            {
                Path.DirectorySeparatorChar
            });
        }

        // path to bin win64
        private string GetAppHostSourceFilePath(string appHostDirectory, string rId)
        {
            string path = Path.Combine(appHostDirectory, (rId == "win-x64") ? "singlefilehost.exe" : "singlefilehost");
            if (! File.Exists(path))
            {
                throw new ApplicationException($"Host file {path} does not exist.");
            }
            return path;

        }
        private string appBinaryFilePath => "Crossroads.Launcher.dll";

    }
}
