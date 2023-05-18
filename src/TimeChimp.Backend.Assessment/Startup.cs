using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using TimeChimp.Backend.Assessment.Repositories;
using TimeChimp.Backend.Assessment.Helpers;

[assembly: ApiConventionType(typeof(DefaultApiConventions))]
namespace TimeChimp.Backend.Assessment
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApi(Configuration);
            services.AddServices();
            services.AddServiceProviders();

            services.AddSingleton<ContextDapper>();
            services.AddMemoryCache();
            services.Configure<CacheConfiguration>(Configuration.GetSection("CacheConfiguration"));
            services.Configure<ConnectionStringOptions>(config => config.SqlConnection = Configuration.GetConnectionString(ConnectionStringOptions.ConnectionStringName));
        }

        public void Configure(IApplicationBuilder app, ILogger<Startup> logger)
        {
            app.UseApi(Configuration, Environment);

            logger.LogInformation(default(EventId), $"{Assembly.GetExecutingAssembly().GetName().Name} started");
        }
    }
}
