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

        public bool IsVersionIconSupported(PackageOption option)
        {
            if(IsLinuxPlatformOrAsTargetRID(option) && HasVersionOrIcon(option))
            {
                return false;
            }
            return true;
        }

        private bool HasVersionOrIcon(PackageOption option)
        {
            return !string.IsNullOrWhiteSpace(option.Version) || !string.IsNullOrWhiteSpace(option.Icon);
        }

        private bool IsLinuxPlatformOrAsTargetRID(PackageOption option)
            => RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || option.TargetOs.Equals(AppHostService.LINUX_RID);
    }
}
