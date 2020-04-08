using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace Verivox.Common
{
    public class Engine : IEngine
    {
        #region Properties

        /// <summary>
        /// Gets or sets service provider
        /// </summary>
        private IServiceProvider _serviceProvider { get; set; }

        #endregion

        #region Utilities

        /// <summary>
        /// Get IServiceProvider
        /// </summary>
        /// <returns>IServiceProvider</returns>
        protected IServiceProvider GetServiceProvider()
        {
            IHttpContextAccessor accessor = ServiceProvider.GetService<IHttpContextAccessor>();
            HttpContext context = accessor.HttpContext;
            return context?.RequestServices ?? ServiceProvider;
        }

        /// <summary>
        /// Run startup tasks
        /// </summary>
        /// <param name="typeFinder">Type finder</param>
        protected virtual void RunStartupTasks(ITypeFinder typeFinder)
        {
            ////find startup tasks provided by other assemblies
            //var startupTasks = typeFinder.FindClassesOfType<IStartupTask>();

            ////create and sort instances of startup tasks
            ////we startup this interface even for not installed plugins. 
            ////otherwise, DbContext initializers won't run and a plugin installation won't work
            //var instances = startupTasks
            //    .Select(startupTask => (IStartupTask)Activator.CreateInstance(startupTask))
            //    .OrderBy(startupTask => startupTask.Order);

            ////execute tasks
            //foreach (var task in instances)
            //    task.Execute();
        }

        /// <summary>
        /// Register dependencies using Autofac
        /// </summary>
        /// <param name="VerivoxConfig">Startup Verivox configuration parameters</param>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="typeFinder">Type finder</param>
        protected virtual IServiceProvider RegisterDependencies(VerivoxConfig dreamlinesConfig, IServiceCollection services, ITypeFinder typeFinder)
        {
            //var autofacServiceProviderFactory = new AutofacServiceProviderFactory(ConfigureContainer);
            //var containerBuilder = autofacServiceProviderFactory.CreateBuilder(services);
            //containerBuilder.Populate(services);
            //containerBuilder.Build();
            //return autofacServiceProviderFactory.CreateServiceProvider(containerBuilder);

            ContainerBuilder containerBuilder = new ContainerBuilder();

            //register engine
            containerBuilder.RegisterInstance(this).As<IEngine>().SingleInstance();

            //register type finder
            containerBuilder.RegisterInstance(typeFinder).As<ITypeFinder>().SingleInstance();

            //find dependency registrars provided by other assemblies
            System.Collections.Generic.IEnumerable<Type> dependencyRegistrars = typeFinder.FindClassesOfType<IDependencyRegistrar>();

            //create and sort instances of dependency registrars
            IOrderedEnumerable<IDependencyRegistrar> instances = dependencyRegistrars
                //.Where(dependencyRegistrar => PluginManager.FindPlugin(dependencyRegistrar)?.Installed ?? true) //ignore not installed plugins
                .Select(dependencyRegistrar => (IDependencyRegistrar)Activator.CreateInstance(dependencyRegistrar))
                .OrderBy(dependencyRegistrar => dependencyRegistrar.Order);

            //register all provided dependencies
            foreach (IDependencyRegistrar dependencyRegistrar in instances)
            {
                dependencyRegistrar.Register(containerBuilder);
            }

            //populate Autofac container builder with the set of registered service descriptors
            containerBuilder.Populate(services);

            //create service provider
            _serviceProvider = new AutofacServiceProvider(containerBuilder.Build());
            return _serviceProvider;
        }

        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            WebAppTypeFinder typeFinder = new WebAppTypeFinder();
            System.Collections.Generic.IEnumerable<Type> startupConfigurations = typeFinder.FindClassesOfType<IVerivoxStartup>();

            //register engine
            containerBuilder.RegisterInstance(this).As<IEngine>().SingleInstance();

            //register type finder
            containerBuilder.RegisterInstance(typeFinder).As<ITypeFinder>().SingleInstance();

            //find dependency registrars provided by other assemblies
            System.Collections.Generic.IEnumerable<Type> dependencyRegistrars = typeFinder.FindClassesOfType<IDependencyRegistrar>();

            //create and sort instances of dependency registrars
            IOrderedEnumerable<IDependencyRegistrar> instances = dependencyRegistrars
                //.Where(dependencyRegistrar => PluginManager.FindPlugin(dependencyRegistrar)?.Installed ?? true) //ignore not installed plugins
                .Select(dependencyRegistrar => (IDependencyRegistrar)Activator.CreateInstance(dependencyRegistrar))
                .OrderBy(dependencyRegistrar => dependencyRegistrar.Order);

            //register all provided dependencies
            foreach (IDependencyRegistrar dependencyRegistrar in instances)
            {
                dependencyRegistrar.Register(containerBuilder);
            }
        }


        #endregion

        #region Methods

        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            //check for assembly already loaded
            Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName == args.Name);
            if (assembly != null)
            {
                return assembly;
            }

            //get assembly from TypeFinder
            ITypeFinder tf = Resolve<ITypeFinder>();
            assembly = tf.GetAssemblies().FirstOrDefault(a => a.FullName == args.Name);
            return assembly;
        }

        /// <summary>
        /// Add and configure services
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration of the application</param>
        /// <returns>Service provider</returns>
        public IServiceProvider ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            //find startup configurations provided by other assemblies
            WebAppTypeFinder typeFinder = new WebAppTypeFinder();
            System.Collections.Generic.IEnumerable<Type> startupConfigurations = typeFinder.FindClassesOfType<IVerivoxStartup>();

            //create and sort instances of startup configurations
            IOrderedEnumerable<IVerivoxStartup> instances = startupConfigurations
                .Select(startup => (IVerivoxStartup)Activator.CreateInstance(startup))
                .OrderBy(startup => startup.Order);

            //configure services
            foreach (IVerivoxStartup instance in instances)
            {
                instance.ConfigureServices(services, configuration);
            }

            //register dependencies
            VerivoxConfig config = services.BuildServiceProvider().GetService<VerivoxConfig>();
            _serviceProvider = RegisterDependencies(config, services, typeFinder);
            //resolve assemblies here. otherwise, plugins can throw an exception when rendering views
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            return _serviceProvider;
        }

        /// <summary>
        /// Resolve dependency
        /// </summary>
        /// <typeparam name="T">Type of resolved service</typeparam>
        /// <returns>Resolved service</returns>
        public T Resolve<T>() where T : class
        {
            return (T)GetServiceProvider().GetRequiredService(typeof(T));
        }

        /// <summary>
        /// Resolve dependency
        /// </summary>
        /// <param name="type">Type of resolved service</param>
        /// <returns>Resolved service</returns>
        public object Resolve(Type type)
        {
            return GetServiceProvider().GetService(type);
        }

        public virtual object ResolveUnregistered(Type type)
        {
            Exception innerException = null;
            foreach (ConstructorInfo constructor in type.GetConstructors())
            {
                try
                {
                    //try to resolve constructor parameters
                    System.Collections.Generic.IEnumerable<object> parameters = constructor.GetParameters().Select(parameter =>
                    {
                        object service = Resolve(parameter.ParameterType);
                        if (service == null)
                        {
                            throw new VerivoxException("Unknown dependency");
                        }

                        return service;
                    });

                    //all is ok, so create instance
                    return Activator.CreateInstance(type, parameters.ToArray());
                }
                catch (Exception ex)
                {
                    innerException = ex;
                }
            }

            throw new VerivoxException("No constructor was found that had all the dependencies satisfied.", innerException);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Service provider
        /// </summary>
        public virtual IServiceProvider ServiceProvider => _serviceProvider;

        #endregion
    }
}
