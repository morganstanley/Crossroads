using System.Collections.Generic;

namespace Crossroads.Core
{
    public class PackageOption
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public string Icon { get; set; }
        public IEnumerable<string> Include { get; set; }
        public string Command { get; set; }
        public string Args { get; set; }
        public string Location { get; set; }
    }
}
