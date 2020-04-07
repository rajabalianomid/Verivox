using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Verivox.Common
{
    public enum DataProviderType
    {
        /// <summary>
        /// Unknown
        /// </summary>
        [EnumMember(Value = "")]
        Unknown,

        /// <summary>
        /// MS SQL Server
        /// </summary>
        [EnumMember(Value = "sqlserver")]
        SqlServer
    }
}
