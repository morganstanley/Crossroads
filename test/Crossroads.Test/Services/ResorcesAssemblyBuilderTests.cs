using Crossroads.Services;
using System;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using System.Threading.Tasks;
using Xunit;

namespace Crossroads.Services.Test
{
    public class ResorcesAssemblyBuilderTests
    {
        private string iconPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "assets", "TestIcon.ico");

        [Theory]
        [InlineData("abc", "2.1.0.0", "good")]
        [InlineData("abc", null, "good")]
        [InlineData(null, null, null)]
        [InlineData("abc", "2.1.0.0", null)]
        [InlineData("abc", "1.2", null)]
        public async Task Build_Success(string targetPath, string version, string iconPath)
        {
            IResourcesAssemblyBuilder builder = GetResourcesAssemblyBuilder();
            var actual = await builder.Build(targetPath, version, iconPath);
            Assert.Equal(targetPath, actual);
        }

        private IResourcesAssemblyBuilder GetResourcesAssemblyBuilder()
        {
            var fileSystem = new MockFileSystem();
            var iconBytes = File.ReadAllBytes(iconPath);
            var iconFile = new MockFileData(iconBytes);
            fileSystem.AddFile("good", iconFile);

            return new ResourcesAssemblyBuilder(fileSystem);
        }

        [Theory]
        [InlineData("abc", null, "bad")]
        [InlineData("abc", "bad", null)]
        public async Task Build_ArgsInvalid_ArgumentException(string targetPath, string version, string iconPath)
        {
            IResourcesAssemblyBuilder builder = GetResourcesAssemblyBuilder();
            await Assert.ThrowsAsync<ArgumentException>(async () => await builder.Build(targetPath, version, iconPath));
        }

        [Fact]
        public async Task Build_VersionInvalid_BuildException()
        {
            IResourcesAssemblyBuilder builder = GetResourcesAssemblyBuilder();
            await Assert.ThrowsAsync<Exception>(async () => await builder.Build("abc", @"1.""2", null));
        }
    }
}
