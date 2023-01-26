using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Crossroads.Test.Utility
{
    public class PlatformRestrictedFactAttribute : FactAttribute
    {
        public PlatformRestrictedFactAttribute(bool Windows = false, bool Linux = false)
        {
            if (isRunningOnWindows && !Windows)
            {
                Skip = "Windows not supported";
            }
            else if (isRunningOnLinux && !Linux)
            {
                Skip = "Linux not supported";
            }
        }

        private static bool isRunningOnWindows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        private static bool isRunningOnLinux => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
    }
}
