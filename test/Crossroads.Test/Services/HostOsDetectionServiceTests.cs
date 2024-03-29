﻿using System.Runtime.InteropServices;
using Crossroads.Services;
using Crossroads.Test.Utility;
using Xunit;

namespace Crossroads.Test.Services
{
    public class HostOsDetectionServiceTests
    {
        [PlatformRestrictedFact(windows: true)]
        public void GetTargetOsRid_ShouldReturnWinRid_WhenRunningOnWindows()
        {
            HostOsDetectionService osService = new HostOsDetectionService();
            var result =  osService.GetTargetOsRid();
            Assert.Equal(AppHostService.WIN_RID, result);
        }

        [PlatformRestrictedFact(linux: true)]
        public void GetTargetOsRid_ShouldReturnLinuxRid_WhenRunningOnLinux()
        {
            HostOsDetectionService osService = new HostOsDetectionService();
            var result =  osService.GetTargetOsRid();
            Assert.Equal(AppHostService.LINUX_RID, result);
        }
    }
}
