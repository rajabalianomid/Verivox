using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verivox.Common;
using Verivox.Common.Plugins;

namespace Verivox.Service
{
    public class PluginService : IPluginService
    {
        private readonly IPluginsInfo _pluginsInfo;
        private readonly IWebHelper _webHelper;
        public PluginService(IWebHelper webHelper)
        {
            _pluginsInfo = Singleton<IPluginsInfo>.Instance;
            _webHelper = webHelper;
        }

        public virtual bool NeedToRestart => _pluginsInfo.PluginNamesToUninstall.Any() || _pluginsInfo.PluginNamesToInstall.Any();

        /// <summary>
        /// Get plugin descriptors
        /// </summary>
        /// <typeparam name="TPlugin">The type of plugins to get</typeparam>
        /// <returns>Plugin descriptors</returns>
        public virtual IEnumerable<PluginDescriptor> GetPluginDescriptors<TPlugin>() where TPlugin : class, IPlugin
        {
            var pluginDescriptors = _pluginsInfo.PluginDescriptors;

            //filter by the passed type
            if (typeof(TPlugin) != typeof(IPlugin))
                pluginDescriptors = pluginDescriptors.Where(descriptor => typeof(TPlugin).IsAssignableFrom(descriptor.PluginType));

            //order by group name
            pluginDescriptors = pluginDescriptors.OrderBy(descriptor => descriptor.Group)
                .ThenBy(descriptor => descriptor.DisplayOrder).ToList();



            return pluginDescriptors;
        }

        public virtual PluginDescriptor GetPluginDescriptorBySystemName<TPlugin>(string systemName) where TPlugin : class, IPlugin
        {
            return GetPluginDescriptors<TPlugin>().FirstOrDefault(descriptor => descriptor.SystemName.Equals(systemName));
        }

        /// <summary>
        /// Prepare plugin to the installation
        /// </summary>
        /// <param name="systemName">Plugin system name</param>
        /// <param name="customer">Customer</param>
        /// <param name="checkDependencies">Specifies whether to check plugin dependencies</param>
        public virtual void PreparePluginToInstall(string systemName)
        {
            //add plugin name to the appropriate list (if not yet contained) and save changes
            if (_pluginsInfo.PluginNamesToInstall.Any(item => item == systemName))
                return;

            _pluginsInfo.PluginNamesToInstall.Add(systemName);
            _pluginsInfo.Save();
            _webHelper.RestartAppDomain();
        }

        /// <summary>
        /// Prepare plugin to the uninstallation
        /// </summary>
        /// <param name="systemName">Plugin system name</param>
        public virtual void PreparePluginToUninstall(string systemName)
        {
            //add plugin name to the appropriate list (if not yet contained) and save changes
            if (_pluginsInfo.PluginNamesToUninstall.Contains(systemName))
                return;
            var descriptor = GetPluginDescriptorBySystemName<IPlugin>(systemName);
            var plugin = descriptor?.Instance<IPlugin>();
            plugin?.PreparePluginToUninstall();

            _pluginsInfo.PluginNamesToUninstall.Add(systemName);
            _pluginsInfo.Save();
            _webHelper.RestartAppDomain();
        }

        public virtual void InstallPlugins()
        {
            //get all uninstalled plugins
            var pluginDescriptors = _pluginsInfo.PluginDescriptors.Where(descriptor => !descriptor.Installed).ToList();

            //filter plugins need to install
            pluginDescriptors = pluginDescriptors.Where(descriptor => _pluginsInfo.PluginNamesToInstall
                .Any(item => item.Equals(descriptor.SystemName))).ToList();
            if (!pluginDescriptors.Any())
                return;

            //install plugins
            foreach (var descriptor in pluginDescriptors.OrderBy(pluginDescriptor => pluginDescriptor.DisplayOrder))
            {
                try
                {
                    //try to install an instance
                    descriptor.Instance<IPlugin>().Install();

                    //remove and add plugin system name to appropriate lists
                    var pluginToInstall = _pluginsInfo.PluginNamesToInstall
                        .FirstOrDefault(plugin => plugin.Equals(descriptor.SystemName));
                    _pluginsInfo.InstalledPluginNames.Add(descriptor.SystemName);
                    _pluginsInfo.PluginNamesToInstall.Remove(pluginToInstall);


                    //mark the plugin as installed
                    descriptor.Installed = true;
                    descriptor.ShowInPluginsList = true;
                }
                catch (Exception exception)
                {
                    //log error
                }
            }

            //save changes
            _pluginsInfo.Save();
        }

        public virtual void UninstallPlugins()
        {
            //get all installed plugins
            var pluginDescriptors = _pluginsInfo.PluginDescriptors.Where(descriptor => descriptor.Installed).ToList();

            //filter plugins need to uninstall
            pluginDescriptors = pluginDescriptors
                .Where(descriptor => _pluginsInfo.PluginNamesToUninstall.Contains(descriptor.SystemName)).ToList();
            if (!pluginDescriptors.Any())
                return;


            //uninstall plugins
            foreach (var descriptor in pluginDescriptors.OrderByDescending(pluginDescriptor => pluginDescriptor.DisplayOrder))
            {
                try
                {
                    //try to uninstall an instance
                    descriptor.Instance<IPlugin>().Uninstall();

                    //remove plugin system name from appropriate lists
                    _pluginsInfo.InstalledPluginNames.Remove(descriptor.SystemName);
                    _pluginsInfo.PluginNamesToUninstall.Remove(descriptor.SystemName);


                    //mark the plugin as uninstalled
                    descriptor.Installed = false;
                    descriptor.ShowInPluginsList = true;
                }
                catch (Exception exception)
                {
                }
            }

            //save changes
            _pluginsInfo.Save();
        }

    }
}
