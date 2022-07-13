/*
 * Morgan Stanley makes this available to you under the Apache License,
 * Version 2.0 (the "License"). You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0.
 *
 * See the NOTICE file distributed with this work for additional information
 * regarding copyright ownership. Unless required by applicable law or agreed
 * to in writing, software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
 * or implied. See the License for the specific language governing permissions
 * and limitations under the License.
 */

using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crossroads.Services
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
