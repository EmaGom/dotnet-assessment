using Microsoft.Extensions.DependencyInjection;
using TimeChimp.Backend.Assessment.Helpers;
using TimeChimp.Backend.Assessment.Managers;
using TimeChimp.Backend.Assessment.Repositories;

namespace TimeChimp.Backend.Assessment
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Extensibility point for adding general services available to all components
        /// </summary>
        /// <param name="services"></param>
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IDataAccessLayer, DataAccessLayerDapper>();
            services.AddScoped<IDataAccessLayer, DataAccessLayerEF>();
            services.AddTransient<IDataAccessLayerFactory, DataAccessLayerFactory>();

            services.AddScoped<IFeedsManager, FeedsManager>();
            services.AddScoped<ICacheService, CacheService>();
        }

        /// <summary>
        /// Extensibility point for adding services constructed using a specialised provider e.g.: country specific services
        /// </summary>
        /// <param name="services"></param>
        public static void AddServiceProviders(this IServiceCollection services)
        {
        }
    }
}
