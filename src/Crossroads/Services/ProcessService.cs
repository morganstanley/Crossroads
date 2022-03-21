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
using System.Diagnostics;
using System.Threading.Tasks;

namespace Crossroads.Services
{
    public class ProcessService : IProcessService
    {
        public async Task<string> GetConsoleOutputAsync(ProcessStartInfo startInfo, int milliseconds)
        {
            string result = string.Empty;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.UseShellExecute = false;

            return await Task.Run(() =>
            {
                var process = Process.Start(startInfo);
                if (process.WaitForExit(milliseconds))
                {
                    var exitCode = process.ExitCode;
                    if (exitCode == 0)
                    {
                        return Task.FromResult(process.StandardOutput.ReadToEnd());
                    }
                    else
                    {
                        throw new Exception($"Package: {startInfo.FileName} failed with exit code {exitCode}");
                    }
                }
                else
                {
                    process.Kill(true);
                    throw new TimeoutException($"Package: {startInfo.FileName}");
                }

            });
        }

        public Task<int> RunAsync(ProcessStartInfo startInfo)
        {
            startInfo.UseShellExecute = false;

            return Task.Run(() =>
            {
                using var process = Process.Start(startInfo);

                process.WaitForExit();

                int exitCode = process.ExitCode;
                return Task.FromResult(exitCode);
            });
        }
    }
}
