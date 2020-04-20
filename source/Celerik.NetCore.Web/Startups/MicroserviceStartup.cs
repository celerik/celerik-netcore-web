using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Celerik.NetCore.Web
{
    public static class MicroserviceStartup
    {
        public static void ConfigureServices(IServiceCollection services, Action setupBusinessServices)
        {
            services.AddHttpContextAccessor();
            setupBusinessServices();
            services.AddControllers().AddNewtonsoftJson();

            services.AddSwaggerGen(setupAction =>
            {
                setupAction.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Sift ShoppingCart SVC",
                    Version = "v1"
                });

                var xmlFile = $"{Assembly.GetEntryAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                setupAction.IncludeXmlComments(xmlPath);
            });

            services.AddMvc();
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            UseSwagger(app);
            UseLocalization(app);
        }

        private static void UseSwagger(IApplicationBuilder app)
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
                setupAction.SwaggerEndpoint("/swagger/v1/swagger.json", "Sift ShoppingCart SVC V1");
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
