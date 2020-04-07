using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verivox.Common.Plugins;

namespace Verivox.Service
{
    /// <summary>
    /// Represents a plugin manager implementation
    /// </summary>
    /// <typeparam name="TPlugin">Type of plugin</typeparam>
    public partial class PluginManager<TPlugin> where TPlugin : class, IPlugin
    {
        IPluginService _pluginService;
        public PluginManager(IPluginService pluginService)
        {
            _pluginService = pluginService;
        }

        #region Methods

        protected IList<TPlugin> LoadAllPlugins(string group)
        {
            if (group == null)
                return new List<TPlugin>();

            return _pluginService.GetPluginDescriptors<IPlugin>().ToList()
             .Where(w => w.Group.Equals(group, StringComparison.InvariantCultureIgnoreCase)).Select(descriptor => descriptor.Instance<TPlugin>())
             .ToList();
        }

        #endregion
    }
}
