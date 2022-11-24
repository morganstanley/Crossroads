using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crossroads.Services
{
    public interface IHostOsDetectionService
    {
        string GetTargetOsRid();
    }
}
