using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PortalGenius.Infrastructure.Data;

namespace PortalGenius.WPF
{
    /// <summary>
    /// This Startup class configures the Service Provider for the Kestrel server only!
    /// The WPF-part has its own Service Provider / Collection configured inside App.xaml.cs.
    /// </summary>
    public class ApiStartup
    {
        private IConfiguration Configuration { get; set; }

        public ApiStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // TODO: Check if local database should be used
            services.AddDbContext<AppDbContext, SQLiteDbContext>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
