using Autofac;
using Microsoft.EntityFrameworkCore;
using Verivox.Common;
using Verivox.Common.Data;
using Verivox.Data;

namespace Verivox.Service.API.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public int Order => 0;

        public void Register(ContainerBuilder builder)
        {
            builder.Register(context => new Context(context.Resolve<DbContextOptions<Context>>())).As<IDbContext>().InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(EfRepository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();

            builder.RegisterType<ProductService>().As<IProductService>().InstancePerLifetimeScope();
            builder.RegisterType<VerivoxFileProvider>().As<IVerivoxFileProvider>().InstancePerDependency();
            builder.RegisterType<PluginService>().As<IPluginService>().InstancePerLifetimeScope();
            builder.RegisterType<WebHelper>().As<IWebHelper>().InstancePerLifetimeScope();
        }
    }
}
