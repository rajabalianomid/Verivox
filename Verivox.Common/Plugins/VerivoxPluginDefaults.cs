﻿namespace Verivox.Common.Plugins
{
    public static partial class VerivoxPluginDefaults
    {
        /// <summary>
        /// Gets the path to file that contained (in previous versions) installed plugin system names
        /// </summary>
        public static string ObsoleteInstalledPluginsFilePath => "~/App_Data/InstalledPlugins.txt";

        /// <summary>
        /// Gets the path to file that contains installed plugin system names
        /// </summary>
        public static string InstalledPluginsFilePath => "~/App_Data/installedPlugins.json";

        /// <summary>
        /// Gets the path to file that contains installed plugin system names
        /// </summary>
        public static string PluginsInfoFilePath => "~/App_Data/plugins.json";

        /// <summary>
        /// Gets the path to plugins folder
        /// </summary>
        public static string Path => "~/Plugins";

        /// <summary>
        /// Gets the plugins folder name
        /// </summary>
        public static string PathName => "Plugins";

        /// <summary>
        /// Gets the path to plugins shadow copies folder
        /// </summary>
        public static string ShadowCopyPath => "~/Plugins/bin";

        /// <summary>
        /// Gets the path to plugins refs folder
        /// </summary>
        public static string RefsPathName => "refs";

        /// <summary>
        /// Gets the name of the plugin description file
        /// </summary>
        public static string DescriptionFileName => "plugin.json";

        /// <summary>
        /// Gets the name of reserve folder for plugins shadow copies
        /// </summary>
        public static string ReserveShadowCopyPathName => "reserve_bin_";

        /// <summary>
        /// Gets the name pattern of reserve folder for plugins shadow copies
        /// </summary>
        public static string ReserveShadowCopyPathNamePattern => "reserve_bin_*";
    }
}
