using Crossroads.Commands;
using Crossroads.Services;
using System.Threading.Tasks;
using Xunit;

namespace Crossroads.Test.Services
{
    public class DisplayHelpPageTests
    {
        [Fact]
        public async Task DisplayHelpPage_WhenCommandIsOfTypeCrossroadsRootCommand_Success()
        {
            var actionType = new CrossroadsRootCommand();
            
            var displayPage = new DisplayHelpPage();

            var result = await displayPage.GetHelpPage(actionType);
            Assert.Equal(0,result);
        }
    }
}
