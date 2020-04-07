using System;
using System.Collections.Generic;
using System.Text;

namespace Verivox.Common
{
    public partial class VerivoxConfig
    {
        public string DataConnectionString { get; set; }
        public bool ClearPluginShadowDirectoryOnStartup { get; set; }
        public bool UsePluginsShadowCopy { get; set; }
        public bool CopyLockedPluginAssembilesToSubdirectoriesOnStartup { get; set; }
        public bool UseUnsafeLoadAssembly { get; set; }
    }
}
