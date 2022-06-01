using Crossroads.Commands;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Text;
using System.Threading.Tasks;

namespace Crossroads.Services
{
    public class HelpPageProxy : IDisplayHelpPage
    {
        public async Task<int> GetHelpPage(RootCommand command)
        {
            var getPage = await CommandExtensions.InvokeAsync(command,"-h");
            return getPage;
        }
    }
}
