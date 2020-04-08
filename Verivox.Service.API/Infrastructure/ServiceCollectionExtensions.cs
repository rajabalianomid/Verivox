using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Verivox.Common;
using Verivox.Data;
using Verivox.Service.API.Infrastructure.Extensions;

namespace Verivox.Service.API.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceProvider ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            //add Config configuration parameters
            VerivoxConfig config = services.ConfigureStartupConfig<VerivoxConfig>(configuration.GetSection("Verivox"));
            //add accessor to HttpContext
            services.AddHttpContextAccessor();

            CommonHelper.DefaultFileProvider = new VerivoxFileProvider(hostingEnvironment);

            IMvcCoreBuilder mvcCoreBuilder = services.AddMvcCore();
            mvcCoreBuilder.PartManager.InitializePlugins(config);

            //create, initialize and configure the engine
            IEngine engine = EngineContext.Create();
            IServiceProvider serviceProvider = engine.ConfigureServices(services, configuration);
            SqlServerDataProvider.InitializeDatabase(config.DataConnectionString);

            engine.Resolve<IPluginService>().InstallPlugins();

            return serviceProvider;
        }
        public static TConfig ConfigureStartupConfig<TConfig>(this IServiceCollection services, IConfiguration configuration) where TConfig : class, new()
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            //create instance of config
            TConfig config = new TConfig();

            //bind it to the appropriate section of configuration
            configuration.Bind(config);

            //and register it as a service
            services.AddSingleton(config);

            return config;
        }
        public static void AddDbContext(this IServiceCollection services)
        {
            services.AddDbContext<Context>(optionsBuilder =>
            {
                VerivoxConfig VerivoxConfig = services.BuildServiceProvider().GetRequiredService<VerivoxConfig>();
                DbContextOptionsBuilder dbContextOptionsBuilder = optionsBuilder.UseLazyLoadingProxies();
                dbContextOptionsBuilder.UseSqlServer(VerivoxConfig.DataConnectionString);
            });
        }
    }
}
