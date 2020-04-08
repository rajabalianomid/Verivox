using Autofac;
using Autofac.Core;
using Verivox.Common;
using Verivox.Common.Data;
using Verivox.Common.Plugins;
using Verivox.Data;
using Verivox.Plugin.ProductConditions.Advance.Data;
using Verivox.Plugin.ProductConditions.Advance.Domain;
using Verivox.Plugin.ProductConditions.Advance.Service;

namespace Verivox.Plugin.ProductConditions.Advance.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public int Order => 0;

        public void Register(ContainerBuilder builder)
        {
            builder.RegisterType<ConditionService>().As<IConditionService>().InstancePerLifetimeScope();

            builder.RegisterPluginDataContext<ConditionContext>("context_product_condition_advance");

            //override required repository with our custom context
            builder.RegisterType<EfRepository<AdvanceCondition>>().As<IRepository<AdvanceCondition>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>("context_product_condition_advance"))
                .InstancePerLifetimeScope();
        }
    }
}
