using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Crossroads.Core
{
    public class ProcessService : IProcessService
    {
        public async Task<string> GetConsoleOutputAsync(ProcessStartInfo startInfo, int millionSeconds)
        {
            string result = string.Empty;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.UseShellExecute = false;

            return await Task.Run(() =>
            {
                var process = Process.Start(startInfo);

                if (process.WaitForExit(millionSeconds))
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
