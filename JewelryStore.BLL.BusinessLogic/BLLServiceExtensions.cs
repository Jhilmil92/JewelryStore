using JewelryStore.DataRepository.Classes;
using JewelryStore.DataRepository.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JewelryStore.BLL
{
    public static class BLLServiceExtensions
    {
        /// <summary>
        /// Configures Dependency Injection for BL layer.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddBLLDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IUnitOfWork,UnitOfWork>();
            return services;
        }
    }
}
