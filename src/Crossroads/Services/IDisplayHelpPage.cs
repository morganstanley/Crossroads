using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Text;
using System.Threading.Tasks;

namespace Crossroads.Services
{
    public interface IDisplayHelpPage
    {
        Task<int> GetHelpPage(RootCommand rootCommand); 
    }
}
