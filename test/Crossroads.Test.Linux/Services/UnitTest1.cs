using System;
using System.IO;
using System.Runtime.InteropServices;
using Xunit;

namespace Crossroads.Test.Linux.Services
{
    public class UnitTest1
    {
        [Fact(Skip = "todo")]
        public void SampleTest()
        {
            var actual = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
            Assert.True(actual);
        }
    }
}