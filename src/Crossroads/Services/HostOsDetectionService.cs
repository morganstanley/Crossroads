using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Crossroads.Services
{
    public class HostOsDetectionService : IHostOsDetectionService
    {
        public string GetTargetOsRid()
        {
            var rid =  RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? "linux-x64" :
                              RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "win-x64" :
                             throw new ArgumentException($"Couldn't detect host OS");
            return rid;
        }
    }
}
