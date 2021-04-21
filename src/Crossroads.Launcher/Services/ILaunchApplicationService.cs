using System.Threading.Tasks;

namespace Crossroads.Launcher.Services
{
    public interface ILaunchApplicationService
    {
        Task<int> RunAsync(string arguments = null);
    }
}