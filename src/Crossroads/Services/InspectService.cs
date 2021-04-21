using Crossroads.Core;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Crossroads.Services
{
    // todo: reuse process service
    public class InspectService : IInspectService
    {
        private readonly IProcessService processService;

        public InspectService(IProcessService processService)
        {
            this.processService = processService;
        }
        public async Task<string> InspectLauncherPackage(string packagePath)
        {
            string result;
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = packagePath,
                    Arguments = "inspect"
                };

                result = await processService.GetConsoleOutputAsync(startInfo, 3000);
            }
            catch(Exception)
            {
                result = $"Package {packagePath} is not valid.";
            }
            return result;
        }
    }
}
