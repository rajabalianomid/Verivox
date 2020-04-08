using System.Collections.Generic;
using Verivox.Common.Plugins;

namespace Verivox.Service
{
    public interface IPluginService
    {
        bool NeedToRestart { get; }
        IEnumerable<PluginDescriptor> GetPluginDescriptors<TPlugin>() where TPlugin : class, IPlugin;
        PluginDescriptor GetPluginDescriptorBySystemName<TPlugin>(string systemName) where TPlugin : class, IPlugin;
        void PreparePluginToInstall(string systemName);
        void PreparePluginToUninstall(string systemName);
        void InstallPlugins();
        void UninstallPlugins();
    }
}
