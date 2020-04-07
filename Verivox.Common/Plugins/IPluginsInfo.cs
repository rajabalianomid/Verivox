﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Verivox.Common.Plugins
{
    /// <summary>
    /// Represents an information about plugins
    /// </summary>
    public interface IPluginsInfo
    {
        /// <summary>
        /// Save plugins info to the file
        /// </summary>
        void Save();

        /// <summary>
        /// Get plugins info
        /// </summary>
        /// <returns>True if data are loaded, otherwise False</returns>
        bool LoadPluginInfo();

        /// <summary>
        /// Create copy from another instance of IPluginsInfo interface
        /// </summary>
        /// <param name="pluginsInfo">Plugins info</param>
        void CopyFrom(IPluginsInfo pluginsInfo);

        /// <summary>
        /// Gets or sets the list of all installed plugin names
        /// </summary>
        IList<string> InstalledPluginNames { get; set; }

        /// <summary>
        /// Gets or sets the list of plugin names which will be uninstalled
        /// </summary>
        IList<string> PluginNamesToUninstall { get; set; }

        /// <summary>
        /// Gets or sets the list of plugin names which will be deleted
        /// </summary>
        IList<string> PluginNamesToDelete { get; set; }

        /// <summary>
        /// Gets or sets the list of plugin names which will be installed
        /// </summary>
        IList<string> PluginNamesToInstall { get; set; }

        /// <summary>
        /// Gets or sets the list of assembly loaded collisions
        /// </summary>
        IList<PluginLoadedAssemblyInfo> AssemblyLoadedCollision { get; set; }

        /// <summary>
        /// Gets or sets a collection of plugin descriptors of all deployed plugins
        /// </summary>
        IEnumerable<PluginDescriptor> PluginDescriptors { get; set; }

        /// <summary>
        /// Gets or sets the list of plugin names which are not compatible with the current version
        /// </summary>
        IList<string> IncompatiblePlugins { get; set; }
    }
}
