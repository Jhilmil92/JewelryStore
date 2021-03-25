using System;
using System.Text;
using JeweleryStore.Common.Enums;
using JewelryStore.BLL;
using JewelryStore.BLL.Classes;
using JewelryStore.BLL.Interfaces;
using JewelryStore.Common.Utilities;
using JewelryStore.DataRepository;
using JewelryStore.DataRepository.Contexts;
using JewelryStoreApp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

namespace JewelryStoreApp
{
    /// <summary>
    /// Startup class for the Web App.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Gets or Sets the Configuration for the Web App
        /// </summary>
        public IConfiguration Configuration { get; set; }

        /// <summary>
        /// Gets or sets the Environment.
        /// </summary>
        public IWebHostEnvironment Environment { get; set; }

        /// <summary>
        /// Creates an instance of <see cref="Startup"/>
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="environment"></param>
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }
        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <remarks>This method gets called by the runtime. Use this method to add services to the container.</remarks>
        /// <remarks>For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940</remarks>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
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
            
            services.AddDataLayerDependencies(Configuration);
            services.AddBLLDependencies(Configuration);
            services.AddScoped<IUserBLL, UserBLL>();
            services.AddScoped<IItemsBLL,ItemsBLL>();
            services.AddScoped<IHttpServices, HttpServices>();
            services.AddHealthChecks()
                .AddCheck<HealthCheck>("example_health_check");
            services.AddControllers();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["JwtSettings:TokenIssuer"],
                    ValidAudience = Configuration["JwtSettings:TokenIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtSettings:Key"]))
                };
            });
            services.AddAuthorization(options =>
                options.AddPolicy("PriviledgedUserOnly",
                policy => policy.RequireClaim("UserType", Enum.GetName(typeof(UserType), UserType.Privileged))));
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(swagger =>
            {
                //This is to generate the Default UI of Swagger Documentation  
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Jwellary Store API",
                    Description = "Calculates the price for Jwellery items"
                });
                // To Enable authorization using Swagger (JWT)  
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}

                    }
                });
            });
        }

        /// <summary>
        /// Configures the app and the environment.
        /// </summary>
        /// <renarks> This method gets called by the runtime. Use this method to configure the HTTP request pipeline.</renarks>
        /// <param name="app"></param>
        /// <param name="env"></param>
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
            app.UseSerilogRequestLogging(); // <-- Add this line

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
            app.UseAuthentication();
            app.UseAuthorization();
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
