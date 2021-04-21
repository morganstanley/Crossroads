using Crossroads.Launcher.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace Crossroads.Launcher.Commands
{
    public class LauncherInspectCommand : Command
    {
        public LauncherInspectCommand()
            :base("inspect")
        {
            // Handler = CommandHandler.Create<IHost>((host) =>
            Handler = CommandHandler.Create<IHost>((host) =>
            {
                var service = host.Services.GetRequiredService<ILauncherInspectService>();
                service.DisplayOption();
                return 0;
            });
        }
    }
}
