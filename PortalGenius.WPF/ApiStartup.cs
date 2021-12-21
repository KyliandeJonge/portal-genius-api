using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PortalGenius.WPF.Data;
using System;

namespace PortalGenius.WPF
{
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
            
            services.AddDbContext<AppDbContext>(options =>
            {
                switch (Configuration.GetSection("DatabaseInUse").Value)
                {
                    case "Sqlite":
                        options.UseSqlite(Configuration.GetConnectionString("Sqlite"));
                        break;
                    case "MSSQL":
                        break;
                    case "PostgreSQL":
                        break;
                    case "Oracle":
                        break;
                    default: throw new ArgumentException("DatabaseInUse missing from appsettings.json");
                }
            });
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
