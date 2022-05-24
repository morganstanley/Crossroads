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

using Crossroads.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace Crossroads.Test.Services
{
    public class QueryRunningModeServiceTests
    {
        [Theory]
        [InlineData("my-command", RunningMode.Launch)]
        [InlineData("", RunningMode.Package)]
        public void QueryRunningMode_Get_Package(string command, RunningMode mode)
        {
            IQueryRunningModeService query = GetQueryRunningModeService(command);
            var actual = query.Query();
            Assert.Equal(mode, actual);
        }

        private IQueryRunningModeService GetQueryRunningModeService(string command)
        {
            IConfiguration configuration = Mock.Of<IConfiguration>(x => x["Launcher:Command"] == command);
            return new QueryRunningModeService(configuration);
        }
    }
}
