using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verivox.Common.Plugins;
using Verivox.Domain.Search;

namespace Verivox.Common
{
    public interface IIntegratedMethod<TEntity, TEntitySearch> : IPlugin
    {
        TEntity ProcessIntegrated(TEntitySearch processRequest);
    }
}
