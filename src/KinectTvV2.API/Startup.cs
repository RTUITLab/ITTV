using System;
using KinectTvV2.API.Core.Configuration;
using KinectTvV2.API.Core.Hubs.KinectTvHub;
using KinectTvV2.API.Core.Providers.S3;
using KinectTvV2.API.Core.Services.Admin;
using KinectTvV2.API.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace KinectTvV2.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            AddDbContext(services);
            
            services.AddControllers();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    var jwtOptions = Configuration.GetSection(nameof(JWTOptions)).Get<JWTOptions>();
                    
                    options.Audience = jwtOptions.Audience;
                    options.TokenValidationParameters.ValidateAudience = true;
                    options.TokenValidationParameters.ValidateLifetime = true;
                    options.RequireHttpsMetadata = false;
                    options.Authority = jwtOptions.Authority;
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "ITTV API", Version = "v1"});

                var securityScheme = new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter into field the Sword 'Bearer' following by space and JWT",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                };

                c.AddSecurityDefinition("Bearer", securityScheme);


                var requirement = new OpenApiSecurityRequirement
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
                        Array.Empty<string>()
                    }
                };
                c.AddSecurityRequirement(requirement);


            });            
            services.AddSignalR();
            RegisterConfiguration(services);
            RegisterServices(services);

        }

        public void AddDbContext(IServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContext<ApplicationDbContext>(x =>
            {
                x.UseNpgsql(
                    Configuration["POSTGRESQL"],
                    npgsql => npgsql.MigrationsAssembly("KinectTvV2.API.Infrastructure.Data"));
            });
        }

        public void RegisterConfiguration(IServiceCollection serviceCollection)
        {
            //TODO: перенести регистрацию клиента S3 в DI (нужно ли это?)
            serviceCollection.Configure<S3Configuration>(Configuration.GetSection(nameof(S3Configuration)));
        }

        public void RegisterServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IAdminService, AdminService>();
            serviceCollection.AddScoped<IS3Provider, S3Provider>();

            serviceCollection.AddScoped<IKinectTvHubHandler, KinectTvHubHandler>();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
            );
            
            app.UseSwagger(c => { c.RouteTemplate = "api/ittv/{documentName}/swagger.json"; });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/api/ittv/v1/swagger.json", "ITTV API");
                c.RoutePrefix = "api/ittv";
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                
                endpoints.MapHub<KinectTvHub>("/hubs/ittv");
            });
        }
    }
}