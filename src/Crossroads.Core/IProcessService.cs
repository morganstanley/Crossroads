using System.Diagnostics;
using System.Threading.Tasks;

namespace Crossroads.Core
{
    public interface IProcessService
    {
        Task<int> RunAsync(ProcessStartInfo startInfo);
        Task<string> GetConsoleOutputAsync(ProcessStartInfo startInfo, int millionSeconds);
    }
}