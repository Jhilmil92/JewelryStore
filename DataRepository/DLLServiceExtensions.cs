using JewelryStore.DataRepository.Classes;
using JewelryStore.DataRepository.Contexts;
using JewelryStore.DataRepository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace JewelryStore.DataRepository
{
    public static class DLLServiceExtensions
    {
        public static IServiceCollection AddDataLayerDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            string connectionString = configuration.GetConnectionString("DefaultConnection");
            var migrationsAssembly = typeof(DLLServiceExtensions).GetTypeInfo().Assembly.GetName().Name;
            services.AddDbContext<JewelryStoreDbContext>(options => options.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));
            return services;
        }
    }
}
