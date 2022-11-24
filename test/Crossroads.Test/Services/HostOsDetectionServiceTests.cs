using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Crossroads.Services;
using Xunit;

namespace Crossroads.Test.Services
{
    public class HostOsDetectionServiceTests
    {
        [Fact]
        public void GetTargetOsRid_Success()
        {
            var expected = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "win-x64" : "linux-x64";
            HostOsDetectionService osService = new HostOsDetectionService();
            var result =  osService.GetTargetOsRid();
            Assert.Equal(expected, result);
        }
    }
}
