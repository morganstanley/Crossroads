using System.Threading.Tasks;

namespace Crossroads.Services
{
    public interface IInspectService
    {
        Task<string> InspectLauncherPackage(string packagePath);
    }
}