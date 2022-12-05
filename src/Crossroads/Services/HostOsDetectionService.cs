using System;
using System.Runtime.InteropServices;

namespace Crossroads.Services
{
    public class HostOsDetectionService : IHostOsDetectionService
    {
        public string GetTargetOsRid()
        {
            return  RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? AppHostService.LINUX_RID :
                              (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? AppHostService.WIN_RID :
                             throw new ArgumentException($"Couldn't detect host OS"));
        }
    }
}
