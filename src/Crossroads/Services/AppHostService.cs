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
using System.Threading.Tasks;

namespace Crossroads.Services
{
    public class AppHostService : IAppHostService
    {
        public async Task ConvertLauncherToBundle(string bundleName, string bundleDirectory, string appHostDirectory, string resourceassemblyPathResult)
        {
            var appHostDestinationFilePath = Path.Combine(appHostDirectory, bundleName);
            await Task.Run(() => HostWriter.CreateAppHost(appHostSourceFilePath, appHostDestinationFilePath, 
                appBinaryFilePath, assemblyToCopyResorcesFrom: resourceassemblyPathResult));

            var bundler = new Bundler(bundleName, bundleDirectory);
            //IReadOnlyList<FileSpec> fee = new List<FileSpec> {
            //    new FileSpec(appHostDirectory, bundleDirectory)
            //};

            //List<FileSpec> list = new List<FileSpec>(files.Length);
            //string[] array = files;
            //foreach (string text in array)
            //{
            //    list.Add(new FileSpec(text, RelativePath(sourceDir, text)));
            //}

            appHostDirectory = Path.GetFullPath(appHostDirectory);
            string[] files = Directory.GetFiles(appHostDirectory, "*", SearchOption.AllDirectories);
            Array.Sort(files, (IComparer<string>?)StringComparer.Ordinal);
            List<FileSpec> list = new List<FileSpec>(files.Length);
            string[] array = files;
            foreach (string text in array)
            {
                list.Add(new FileSpec(text, RelativePath(appHostDirectory, text)));
            }

            await Task.Run(() => bundler.GenerateBundle(list));


            //bundleDirectory - null
            //appHostDirectory - "C:\\Users\\User\\AppData\\Local\\Temp\\crossroads\\hdp2i4h0.neh\\AppDirectory"
            //appBinaryFilePath - Crossroads.dll
            //bundleName - "newnotepad.exe"
            //appHostDestinationFilePath - "C:\\Users\\User\\AppData\\Local\\Temp\\crossroads\\hdp2i4h0.neh\\AppDirectory\\newnotepad.exe"
            //appHostSourceFilePath - C:\Users\User\Downloads\dev\Crossroads\src\Crossroads\bin\Debug\net6.0\win-x64\AppHost\apphost.exe

            //await Task.Run(() => bundler.GenerateBundle(appHostDirectory));

        }

        //public string GenerateBundle2(string sourceDir)
        //{
        //    sourceDir = Path.GetFullPath(sourceDir);
        //    string[] files = Directory.GetFiles(sourceDir, "*", SearchOption.AllDirectories);
        //    Array.Sort(files, (IComparer<string>?)StringComparer.Ordinal);
        //    List<FileSpec> list = new List<FileSpec>(files.Length);
        //    string[] array = files;
        //    foreach (string text in array)
        //    {
        //        list.Add(new FileSpec(text, RelativePath(sourceDir, text)));
        //    }
        //    return GenerateBundle2(list);
        //}

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

        private string appHostSourceFilePath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AppHost", "apphost.exe");
        private string appBinaryFilePath => "Crossroads.dll";
    }
}
