using System;
using System.Runtime.InteropServices;

namespace Crossroads.Services
{
    public class HostOsDetectionService : IHostOsDetectionService
    {
        public string GetTargetOsRid()
        {
            return  RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? "linux-x64" :
                              (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "win-x64" :
                             throw new ArgumentException($"Couldn't detect host OS"));
        }
    }
}
