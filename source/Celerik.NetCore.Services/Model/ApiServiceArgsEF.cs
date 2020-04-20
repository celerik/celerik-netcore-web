using System;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Celerik.NetCore.Services
{
    /// <summary>
    /// Encapsulates the properties to initialize a new ApiServiceEF&lt;TDbContext&gt;.
    /// </summary>
    /// <typeparam name="TLoggerCategory">The type who's name is used
    /// for the logger category name.</typeparam>
    /// <typeparam name="TDbContext">The type of DbContext.</typeparam>
    public class ApiServiceArgsEF<TLoggerCategory, TDbContext>
        : ApiServiceArgs<TLoggerCategory>
            where TDbContext : DbContext
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
        /// <param name="dbContext">Reference to the current DbContext
        /// instance.</param>
        public ApiServiceArgsEF(
            IServiceProvider serviceProvider,
            IConfiguration config,
            IStringLocalizerFactory stringLocalizerFactory,
            ILogger<TLoggerCategory> logger,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            TDbContext dbContext)
            : base(serviceProvider, config, stringLocalizerFactory, logger, mapper, httpContextAccessor)
            => DbContext = dbContext;

        /// <summary>
        /// Reference to the current DbContext instance.
        /// </summary>
        public TDbContext DbContext { get; private set; }
    }
}
