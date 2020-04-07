using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Verivox.Common;
using Verivox.Data;

namespace Verivox.Service.API.Infrastructure
{
    public class DbStartup : IVerivoxStartup
    {
        public int Order => 10;

        public void Configure(IApplicationBuilder application)
        {
        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextPool<Context>(optionsBuilder =>
            {
                optionsBuilder.UseLazyLoadingProxies();
            });
        }
    }
}
