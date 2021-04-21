using System.Threading.Tasks;

namespace Crossroads.Services
{
    public interface IResourcesAssemblyBuilder
    {
        Task<string> Build(string targetPath, string version = null, string iconPath = null);
    }
}
