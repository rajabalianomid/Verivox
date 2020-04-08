using Autofac;
using Autofac.Core;
using Verivox.Common;
using Verivox.Common.Data;
using Verivox.Common.Plugins;
using Verivox.Data;
using Verivox.Plugin.ProductConditions.Basic.Data;
using Verivox.Plugin.ProductConditions.Basic.Domain;
using Verivox.Plugin.ProductConditions.Basic.Service;

namespace Verivox.Plugin.ProductConditions.Basic.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public int Order => 0;

        public void Register(ContainerBuilder builder)
        {
            builder.RegisterType<ConditionService>().As<IConditionService>().InstancePerLifetimeScope();

            builder.RegisterPluginDataContext<ConditionContext>("context_product_condition_basic");

            //override required repository with our custom context
            builder.RegisterType<EfRepository<Condition>>().As<IRepository<Condition>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>("context_product_condition_basic"))
                .InstancePerLifetimeScope();
        }
    }
}
