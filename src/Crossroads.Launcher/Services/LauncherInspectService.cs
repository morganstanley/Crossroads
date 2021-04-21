using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crossroads.Launcher.Services
{
    public class LauncherInspectService : ILauncherInspectService
    {
        private readonly IConfiguration configuration;

        public LauncherInspectService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void DisplayOption()
        {
            StringBuilder resultBuilder = new StringBuilder();
            resultBuilder.AppendLine("Crossroads Inspect");
            showLauncherOption(resultBuilder, "Name");
            showLauncherOption(resultBuilder, "Command");
            showLauncherOption(resultBuilder, "Args");
            showLauncherOption(resultBuilder, "Version");
            showLauncherOption(resultBuilder, "Icon");
            showLauncherOption(resultBuilder, "Location");

            var includes = configuration.GetSection("Launcher:Include")?.Get<IEnumerable<string>>();
            if (includes != null && includes.Any())
            {
                resultBuilder.AppendLine("Include:");
                foreach(var include in includes)
                {
                    resultBuilder.AppendLine($"  {include}");
                }
            }
            Console.WriteLine(resultBuilder.ToString());
        }

        private void showLauncherOption(StringBuilder stringBuilder, string name)
        {
            var configValue = configuration[$"Launcher:{name}"];
            if (!string.IsNullOrWhiteSpace(configValue))
            {
                stringBuilder.AppendLine($"{name}: {configValue}");
            }
        }
    }
}
