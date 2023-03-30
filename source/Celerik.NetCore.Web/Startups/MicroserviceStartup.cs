using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Celerik.NetCore.Web
{
    /// <summary>
    /// The class containing the startup methods for the application.
    /// </summary>
    public static class MicroserviceStartup
    {
        /// <summary>
        /// Use this method to add services to the container.
        /// </summary>
        /// <param name="services">Specifies the contract for a collection of
        /// service descriptors.</param>
        public static void ConfigureServices(
            IServiceCollection services,
            IConfiguration config,
            string apiName,
            Action setupBusinessServices)
        {
            services.AddHttpContextAccessor();
            setupBusinessServices();
            services.AddControllers().AddNewtonsoftJson();

            services.AddSwaggerGen(setupAction =>
            {
                setupAction.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = apiName,
                    Version = "v1"
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                var addAuthHeader = config["SwaggerConfig:AddAuthorizationHeader"];
                if (addAuthHeader != null && addAuthHeader.ToLower() == "true")
                {
                    setupAction.OperationFilter<AuthorizationHeader>();
                }

                setupAction.IncludeXmlComments(xmlPath);
            });
        }

        /// <summary>
        /// Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">Provides the mechanisms to configure an
        /// application's request pipeline.</param>
        /// <param name="env">Provides information about the web hosting
        /// environment an application is running in.</param>
        public static void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            IConfiguration config,
            string apiName)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();
            app.UseRouting();

            var addAuthHeader = config["SwaggerConfig:AddAuthorizationHeader"];
            if (addAuthHeader != null && addAuthHeader.ToLower() == "true")
            {
                app.UseAuthorization();
                app.Use((httpContext, next) =>
                {
                    if (httpContext.Request.Headers["X-Authorization"].Any())
                    {
                        httpContext.Request.Headers.Add("Authorization", httpContext.Request.Headers["X-Authorization"]);
                    }
                    return next();
                });
            }

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            UseSwagger(app, apiName);
            UseLocalization(app);
        }

        private static void UseSwagger(IApplicationBuilder app, string apiName)
        {
            app.UseSwagger(setupAction =>
            {
                setupAction.PreSerializeFilters.Add((swagger, httpReq) =>
                {
                    swagger.Servers = new List<OpenApiServer> {
                        new OpenApiServer {
                            Url = $"{httpReq.Scheme}://{httpReq.Host.Value}"
                        }
                    };
                });
            });

            app.UseSwaggerUI(setupAction =>
            {
                setupAction.SwaggerEndpoint("/swagger/v1/swagger.json", apiName);
                setupAction.RoutePrefix = string.Empty;
            });
        }

        private static void UseLocalization(IApplicationBuilder app)
        {
            var supportedCultures = new List<CultureInfo>{
                new CultureInfo("en"),
                new CultureInfo("es")
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });
        }
    }
}
