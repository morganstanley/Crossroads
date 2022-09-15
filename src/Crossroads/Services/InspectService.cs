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

using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Crossroads.Services
{
    // todo: reuse process service
    public class InspectService : IInspectService
    {
        private readonly IProcessService processService;

        public InspectService(IProcessService processService)
        {
            this.processService = processService;
        }
        public async Task<string> InspectLauncherPackage(string packagePath)
        {
            string result;
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = packagePath,
                    Arguments = "inspect"
                };
                result = await processService.GetConsoleOutputAsync(startInfo, 3000);
            }
            catch(Exception)
            {
                result = $"Package {packagePath} is not valid.";
            }
            return result;
        }
    }
}
