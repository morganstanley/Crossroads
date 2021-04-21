using Crossroads.Core;
using System;
using System.Threading.Tasks;

namespace Crossroads.Services
{
    public interface IPackageApplicationBuilder: IDisposable
    {
        Task Build(PackageOption option);
    }
}