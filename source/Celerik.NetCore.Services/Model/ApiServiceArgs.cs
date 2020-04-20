using System;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Celerik.NetCore.Services
{
    /// <summary>
    /// Encapsulates the properties to initialize a new ApiService.
    /// </summary>
    /// <typeparam name="TLoggerCategory">The type who's name is used
    /// for the logger category name.</typeparam>
    public class ApiServiceArgs<TLoggerCategory>
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="serviceProvider">Reference to the current IServiceProvider
        /// instance.</param>
        /// <param name="config">Reference to the current IConfiguration
        /// instance.</param>
        /// <param name="stringLocalizerFactory">Reference to the current
        /// IStringLocalizerFactory.</param>
        /// <param name="logger">Reference to the current ILogger
        /// instance.</param>
        /// <param name="mapper">Reference to the current IMapper
        /// instance.</param>
        /// <param name="httpContextAccessor">Reference to the current IHttpContextAccessor
        /// instance.</param>
        public ApiServiceArgs(
            IServiceProvider serviceProvider,
            IConfiguration config,
            IStringLocalizerFactory stringLocalizerFactory,
            ILogger<TLoggerCategory> logger,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            ServiceProvider = serviceProvider;
            Config = config;
            StringLocalizerFactory = stringLocalizerFactory;
            Logger = logger;
            Mapper = mapper;
            HttpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Reference to the current IServiceProvider instance.
        /// </summary>
        public IServiceProvider ServiceProvider { get; private set; }

        /// <summary>
        /// Reference to the current IConfiguration instance.
        /// </summary>
        public IConfiguration Config { get; private set; }

        /// <summary>
        /// Factory to create IStringLocalizer objects.
        /// </summary>
        public IStringLocalizerFactory StringLocalizerFactory { get; private set; }

        /// <summary>
        /// Reference to the current ILogger instance.
        /// </summary>
        public ILogger<TLoggerCategory> Logger { get; private set; }

        /// <summary>
        /// Reference to the current IMapper instance.
        /// </summary>
        public IMapper Mapper { get; private set; }

        /// <summary>
        /// Reference to the current IHttpContextAccessor instance.
        /// </summary>
        public IHttpContextAccessor HttpContextAccessor { get; private set; }
    }
}
