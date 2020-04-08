using Verivox.Common.Plugins;

namespace Verivox.Common
{
    public interface IIntegratedMethod<TEntity, TEntitySearch> : IPlugin
    {
        TEntity ProcessIntegrated(TEntitySearch processRequest);
    }
}
