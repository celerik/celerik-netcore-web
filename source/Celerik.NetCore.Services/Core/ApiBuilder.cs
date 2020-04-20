using System;
using System.Collections.Generic;
using AutoMapper;
using Celerik.NetCore.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Celerik.NetCore.Services
{
    /// <summary>
    /// Builder to add core services to the passed-in service collection.
    /// </summary>
    /// <typeparam name="TLoggerCategory">The type who's name is used
    /// for the logger category name.</typeparam>
    /// <typeparam name="TDbContext">The type of DbContext.</typeparam>
    public class ApiBuilder<TLoggerCategory, TDbContext>
        where TDbContext : DbContext
    {
        /// <summary>
        /// Reference to the current IServiceCollection instance.
        /// </summary>
        private readonly IServiceCollection _services;

        /// <summary>
        /// Reference to the current ServiceProvider instance.
        /// </summary>
        private readonly ServiceProvider _serviceProvider;

        /// <summary>
        /// Reference to the current IConfiguration instance.
        /// </summary>
        private readonly IConfiguration _config;

        /// <summary>
        /// Object with the configuration needed to add core services.
        /// </summary>
        private readonly ApiConfig _apiConfig;

        /// <summary>
        /// List containing the names of the methods already executed.
        /// </summary>
        private readonly List<string> _invokedMethods;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="services">Reference to the current IServiceCollection
        /// instance.</param>
        public ApiBuilder(IServiceCollection services)
        {
            _services = services;
            _serviceProvider = services.BuildServiceProvider();
            _config = _serviceProvider.GetRequiredService<IConfiguration>();
            _apiConfig = new ApiConfig();
            _invokedMethods = new List<string>();
        }

        /// <summary>
        /// Gets the current service type.
        /// </summary>
        public ApiServiceType ServiceType => _config.GetServiceType();

        /// <summary>
        /// Adds localization to the current service collection.
        /// </summary>
        /// <returns>Reference to the current ApiBuilder.</returns>
        /// <exception cref="InvalidOperationException">If this method
        /// was already called.</exception>
        internal ApiBuilder<TLoggerCategory, TDbContext> AddLocalization()
        {
            if (IsInvoked(nameof(AddLocalization)))
                throw new InvalidOperationException(
                    ServiceResources.Get("ApiBuilder.MethodAlreadyCalled", nameof(AddLocalization))
                );

            _services.AddLocalization(opts =>
            {
                if (_apiConfig.ResourcesPath != null)
                    opts.ResourcesPath = _apiConfig.ResourcesPath;
            });

            _invokedMethods.Add(nameof(AddLocalization));
            return this;
        }

        /// <summary>
        /// Sets the current configuration.
        /// </summary>
        /// <param name="configure">The configure callback.</param>
        /// <returns>Reference to the current ApiBuilder.</returns>
        /// <exception cref="InvalidOperationException">If this method
        /// was already called.</exception>
        internal ApiBuilder<TLoggerCategory, TDbContext> SetConfiguration(
            Action<IConfiguration, ApiConfig> configure = null)
        {
            if (IsInvoked(nameof(SetConfiguration)))
                throw new InvalidOperationException(
                    ServiceResources.Get("ApiBuilder.MethodAlreadyCalled", nameof(SetConfiguration))
                );

            configure?.Invoke(_config, _apiConfig);

            _invokedMethods.Add(nameof(SetConfiguration));
            return this;
        }

        /// <summary>
        /// Adds logging to the current service collection.
        /// </summary>
        /// <returns>Reference to the current ApiBuilder.</returns>
        /// <exception cref="InvalidOperationException">If this method
        /// was already called.</exception>
        internal ApiBuilder<TLoggerCategory, TDbContext> AddLogging()
        {
            if (IsInvoked(nameof(AddLogging)))
                throw new InvalidOperationException(
                    ServiceResources.Get("ApiBuilder.MethodAlreadyCalled", nameof(AddLogging))
                );

            _services.AddLogging(configure =>
            {
                configure.ClearProviders();
                configure.AddConsole(console =>
                {
                    if (_apiConfig.LoggingTimestampFormat != null)
                        console.TimestampFormat = _apiConfig.LoggingTimestampFormat;
                });
                configure.AddDebug();
            });

            _invokedMethods.Add(nameof(AddLogging));
            return this;
        }

        /// <summary>
        /// Checks if a SqlServer DbContext should be added to the
        /// current service collection. It is added only if the
        /// SqlServerConnectionStringKey is defined in the config
        /// file and the service type is ApiServiceType.ServceEF.
        /// </summary>
        /// <returns>Reference to the current ApiBuilder.</returns>
        /// <exception cref="InvalidOperationException">If this method
        /// was already called.</exception>
        /// <exception cref="ConfigException">If the connection
        /// string name is not found either in the environment variables
        /// or in the config file.</exception>
        internal ApiBuilder<TLoggerCategory, TDbContext> CheckSqlServer()
        {
            if (IsInvoked(nameof(CheckSqlServer)))
                throw new InvalidOperationException(
                    ServiceResources.Get("ApiBuilder.MethodAlreadyCalled", nameof(CheckSqlServer))
                );
            if (_apiConfig.SqlServerConnectionStringKey == null)
                return this;
            if (ServiceType != ApiServiceType.ServiceEF)
                return this;

            var connectionStringSection = _config.GetSection(
                _apiConfig.SqlServerConnectionStringKey);

            if (connectionStringSection.Exists())
            {
                var connectionStringName = connectionStringSection.Value;
                var connectionStringValue = _config[connectionStringName];

                if (string.IsNullOrEmpty(connectionStringValue))
                    throw new ConfigException(
                        UtilResources.Get("Common.EnvironmentVariableNotFound", connectionStringValue));

                _services.AddDbContext<TDbContext>(
                    options => options.UseSqlServer(connectionStringValue)
                );
            }

            _invokedMethods.Add(nameof(CheckSqlServer));
            return this;
        }

        /// <summary>
        /// Adds automapper to the current service collection.
        /// </summary>
        /// <param name="configure">The configure callback.</param>
        /// <returns>Reference to the current ApiBuilder.</returns>
        /// <exception cref="InvalidOperationException">If this method
        /// was already called.</exception>
        public ApiBuilder<TLoggerCategory, TDbContext> AddAutomapper(
            Action<IMapperConfigurationExpression> configure = null)
        {
            if (IsInvoked(nameof(AddAutomapper)))
                throw new InvalidOperationException(
                    ServiceResources.Get("ApiBuilder.MethodAlreadyCalled", nameof(AddAutomapper))
                );

            var config = new MapperConfiguration(options =>
            {
                configure?.Invoke(options);
            });

            var mapper = config.CreateMapper();
            _services.AddSingleton(mapper);

            _invokedMethods.Add(nameof(AddAutomapper));
            return this;
        }

        /// <summary>
        /// Adds fluent validation to the current service collection.
        /// </summary>
        /// <param name="configure">The configure callback.</param>
        /// <returns>Reference to the current ApiBuilder.</returns>
        /// <exception cref="InvalidOperationException">If this method
        /// was already called.</exception>
        /// <exception cref="ArgumentNullException">Configure is null.
        ///</exception>
        public ApiBuilder<TLoggerCategory, TDbContext> AddValidators(
            Action configure)
        {
            if (IsInvoked(nameof(AddValidators)))
                throw new InvalidOperationException(
                    ServiceResources.Get("ApiBuilder.MethodAlreadyCalled", nameof(AddValidators))
                );

            if (configure == null)
                throw new ArgumentNullException(
                    UtilResources.Get("Common.ArgumentCanNotBeNull", nameof(configure))
                );

            _services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            configure.Invoke();

            _invokedMethods.Add(nameof(AddValidators));
            return this;
        }

        /// <summary>
        /// Adds business services to the current service collection.
        /// </summary>
        /// <param name="configure">The configure callback.</param>
        /// <returns>Reference to the current ApiBuilder.</returns>
        /// <exception cref="InvalidOperationException">If this method
        /// was already called.</exception>
        /// <exception cref="ArgumentNullException">Configure is null.
        ///</exception>
        public ApiBuilder<TLoggerCategory, TDbContext> AddBusinesServices(
            Action configure)
        {
            if (IsInvoked(nameof(AddBusinesServices)))
                throw new InvalidOperationException(
                    ServiceResources.Get("ApiBuilder.MethodAlreadyCalled", nameof(AddBusinesServices))
                );

            if (configure == null)
                throw new ArgumentNullException(
                    UtilResources.Get("Common.ArgumentCanNotBeNull", nameof(configure))
                );

            switch (ServiceType)
            {
                case ApiServiceType.ServiceEF:
                    _services.AddTransient<ApiServiceArgsEF<TLoggerCategory, TDbContext>>();
                    break;
                case ApiServiceType.ServiceMock:
                    _services.AddTransient<ApiServiceArgs<TLoggerCategory>>();
                    break;
            }

            configure.Invoke();

            _invokedMethods.Add(nameof(AddBusinesServices));
            return this;
        }

        /// <summary>
        /// Indicates wheather the passed-in method name was already
        /// executed in this builder.
        /// </summary>
        /// <param name="methodName">The name of the method.</param>
        /// <returns>True if the passed-in method name was already
        /// executed in this builder.</returns>
        private bool IsInvoked(string methodName)
            => _invokedMethods.Contains(methodName);
    }
}
