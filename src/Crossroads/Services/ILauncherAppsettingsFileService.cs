using Crossroads.Core;
using System.Threading.Tasks;

namespace Crossroads.Services
{
    public interface ILauncherAppsettingsFileService
    {
        Task SetOption(string filePath, PackageOption option);
    }
}