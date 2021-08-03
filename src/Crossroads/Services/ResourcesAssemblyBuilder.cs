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

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using System;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;

namespace Crossroads.Services
{
    public class ResourcesAssemblyBuilder : IResourcesAssemblyBuilder
    {
        private readonly IFileSystem fileSystem;

        public ResourcesAssemblyBuilder(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        private Stream GetWin32Resources(Compilation compilation, string version, string iconPath)
        {
            try
            {
                using var iconInIcoFormat = GetIconStream(iconPath);
                return compilation.CreateDefaultWin32Resources(NeedVersion(version), true, null, iconInIcoFormat);
            }
            catch(IOException exception)
            {
                throw new ArgumentException($"Icon {iconPath} is invalid.", exception);
            }
        }

        private Stream GetIconStream(string iconPath)
        {
            Stream iconStream = null;
            if (NeedIcon(iconPath))
            {
                try
                {
                    iconStream = fileSystem.File.OpenRead(iconPath);
                }
                catch (FileNotFoundException exception)
                {
                    throw new FileNotFoundException("Invalid icon path.", exception);
                }
            }
            return iconStream;
        }

        public async Task<string> Build(string targetPath, string version = null, string iconPath = null)
        {
            string resultPath = null;
            if (NeedBuild(version, iconPath))
            {
                using Stream outputAssembly = fileSystem.File.Create(targetPath);
                var syntaxTree = SyntaxFactory.ParseSyntaxTree(SourceText.From(ConsoleAppSourceText(version)), new CSharpParseOptions());
                var compilation = CSharpCompilation.Create("Crossroads.ResorcesAssembly.exe")
                    .WithOptions(new CSharpCompilationOptions(OutputKind.ConsoleApplication)
                        .WithOptimizationLevel(OptimizationLevel.Release)
                        .WithPlatform(Platform.AnyCpu)
                    ).AddReferences(
                        MetadataReference.CreateFromFile(typeof(object).Assembly.Location)
                    )
                    .AddSyntaxTrees(syntaxTree);
                using var win32Resources = GetWin32Resources(compilation, version, iconPath);

                var emitResult = await Task.Run(() => compilation.Emit(outputAssembly, win32Resources: win32Resources));

                if (emitResult.Success)
                {
                    resultPath = targetPath;
                }
                else
                {
                    var compielErrors = emitResult.Diagnostics.Where(x => x.Severity == DiagnosticSeverity.Error).ToArray();
                    if (compielErrors.Any(x => x.Id == "CS7034"))
                    {
                        throw new ArgumentException($"Invalid version: {version}");
                    }
                    else
                    {
                        throw new Exception(compielErrors.First().GetMessage());
                    }
                }
                    
            }
            return resultPath;
        }

        private bool NeedBuild(string version, string iconPath)
        {
            return NeedVersion(version) || NeedIcon(iconPath);
        }

        private string VersionSourceText(string version) => NeedVersion(version) ? $@"
using System.Reflection;
[assembly: AssemblyVersion(""{version}"")]
" : string.Empty;

        private string ConsoleAppSourceText(string version) => $@"
{VersionSourceText(version)}

class Program
{{
    static void Main(string[] args)
    {{
    }}
}}
";

        private bool NeedIcon(string iconPath) => !string.IsNullOrWhiteSpace(iconPath);
        private bool NeedVersion(string version) => !string.IsNullOrWhiteSpace(version);

        // public bool NeedBuild => NeedIcon || NeedVersion;
    }
}
