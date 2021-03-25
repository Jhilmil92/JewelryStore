using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using JewelryStore.BLL;
using JewelryStore.BLL.Classes;
using JewelryStore.BLL.Interfaces;
using JewelryStore.Common.Utilities;
using JewelryStore.DataRepository;
using JewelryStore.DataRepository.Contexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JewelryStoreApp
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }
        public IWebHostEnvironment Environment { get; set; }
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //string connectionString = Configuration.GetConnectionString("DefaultConnection");
            //var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            //services.AddDbContext<JewelryStoreDbContext>(options => options.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", corsBuilder =>
                {
                    corsBuilder.AllowAnyHeader()
                    .AllowAnyMethod()
                    .SetIsOriginAllowed(origin => origin == "http://localhost:4200")
                    .AllowCredentials();
                });
            });
            //services.AddControllers(options =>
            //{
            //    var policy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
            //    .RequireAuthenticatedUser()
            //    .Build();
            //    options.Filters.Add(new AuthorizeFilter(policy));
            //});
            
            services.AddDataLayerDependencies(Configuration);
            services.AddBLLDependencies(Configuration);
            services.AddScoped<IUserBLL, UserBLL>();
            services.AddScoped<IItemsBLL,ItemsBLL>();
            services.AddHealthChecks()
                .AddCheck<HealthCheck>("example_health_check");
            services.AddControllers();
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            bool seed = Configuration.GetSection("Data").GetValue<bool>("Seed");
            if (seed)
            {
                InitializeDatabase(app); 
                throw new Exception("Seeding completed, disable the seed flag in appsettings.json");
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            app.UseRouting();
            app.UseCors("CorsPolicy");
            //app.UseAuthentication();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health");
                endpoints.MapControllers();
                endpoints.MapDefaultControllerRoute();
            });
        }

        private void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<JewelryStoreDbContext>();
                context.Database.Migrate();
                
                context.SaveChanges();
            }
        }

    }
}
