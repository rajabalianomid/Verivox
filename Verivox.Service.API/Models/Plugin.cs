using System.Collections.Generic;

namespace Verivox.Service.API.Models
{
    public class Plugin
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Installed { get; set; }
    }
    public class PluginCollection
    {
        public IEnumerable<Plugin> Plugins { get; set; }
        public bool NeedToRestart { get; set; }
    }
}
