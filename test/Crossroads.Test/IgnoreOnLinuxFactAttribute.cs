using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Crossroads.Test
{
    public class IgnoreOnLinuxFactAttribute: FactAttribute
    {
        public IgnoreOnLinuxFactAttribute()
        {
            if (!isRunningOnWindows())
            {
                Skip = "Ignore non-windows OS";
            }
        }

        private static bool isRunningOnWindows()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        }
    }
}
