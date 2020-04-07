using System;
using System.Collections.Generic;
using System.Text;

namespace Verivox.Common
{
    /// <summary>
    /// Reader/Write locker type
    /// </summary>
    public enum ReaderWriteLockType
    {
        Read,
        Write,
        UpgradeableRead
    }
}
