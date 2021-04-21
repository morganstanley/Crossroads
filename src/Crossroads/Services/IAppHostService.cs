using System.Threading.Tasks;

namespace Crossroads.Services
{
    public interface IAppHostService
    {
        Task ConvertLauncherToBundle(string bundleName, string bundleDirectory, string appHostDirectory, string resourceassemblyPathResult);
    }
}