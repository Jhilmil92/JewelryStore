using JewelryStore.DataRepository.Classes;
using JewelryStore.DataRepository.Contexts;
using JewelryStore.DataRepository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace JewelryStore.BLL
{
    public static class BLLServiceExtensions
    {
        public static IServiceCollection AddBLLDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            //string connectionString = configuration.GetConnectionString("DefaultConnection");
            //var migrationsAssembly = typeof(BLLServiceExtensions).GetTypeInfo().Assembly.GetName().Name;
            //services.AddDbContext<JewelryStoreDbContext>(options => options.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));
            services.AddTransient<IUnitOfWork,UnitOfWork>();
            return services;
        }
    }
}
