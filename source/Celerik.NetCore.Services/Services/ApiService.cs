using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Celerik.NetCore.Util;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Celerik.NetCore.Services
{
    /// <summary>
    /// Base class for all services.
    /// </summary>
    /// <typeparam name="TLoggerCategory">The type who's name is used
    /// for the logger category name.</typeparam>
    public abstract class ApiService<TLoggerCategory> : IDisposable
    {
        /// <summary>
        /// Indicates wheter Dispose() was already called.
        /// </summary>
        private bool _isDisposed = false;

        /// <summary>
        /// Object to measure time executions.
        /// </summary>
        private Stopwatch _stopWatch = null;

        /// <summary>
        /// Reference to te current IHttpContextAccessor instance.
        /// </summary>
        private IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="args">Encapsulates the properties to initialize a new
        /// ApiServiceArgs&lt;TLoggerCategory&gt;.</param>
        /// <exception cref="ArgumentNullException">Args is null.</exception>
        public ApiService(ApiServiceArgs<TLoggerCategory> args)
        {
            if (args == null)
                throw new ArgumentNullException(
                    UtilResources.Get("Common.ArgumentCanNotBeNull", nameof(args)));

            _stopWatch = new Stopwatch();
            _httpContextAccessor = args.HttpContextAccessor;

            ServiceProvider = args.ServiceProvider;
            Config = args.Config;
            Logger = args.Logger;
            Mapper = args.Mapper;

            UtilResources.Initialize(args.StringLocalizerFactory);
        }

        /// <summary>
        /// Reference to the current IServiceProvider instance.
        /// </summary>
        protected IServiceProvider ServiceProvider { get; private set; }

        /// <summary>
        /// Reference to the current IConfiguration instance.
        /// </summary>
        protected IConfiguration Config { get; private set; }

        /// <summary>
        /// Reference to the current ILogger instance.
        /// </summary>
        protected ILogger<TLoggerCategory> Logger { get; private set; }

        /// <summary>
        /// Reference to the current IMapper instance.
        /// </summary>
        protected IMapper Mapper { get; private set; }

        /// <summary>
        /// Gets a reference to the current HttpContext.
        /// </summary>
        protected HttpContext HttpContext => _httpContextAccessor?.HttpContext;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting
        /// unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting
        /// unmanaged resources.
        /// </summary>
        /// <param name="disposing">Indicates whether it is disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed)
                return;

            var type = GetType().Name;
            Logger.LogDebug(ServiceResources.Get("ApiService.Dispose.Disposing"), type);

            if (disposing)
            {
                _stopWatch = null;
                _httpContextAccessor = null;

                ServiceProvider = null;
                Config = null;
                Logger = null;
                Mapper = null;
            }

            _isDisposed = true;
        }

        /// <summary>
        /// Starts logging an action.
        /// </summary>
        protected void StartLog()
        {
            _stopWatch.Start();
            var callerMethodName = new StackTrace(1).GetMethodName();
            Logger.LogDebug(ServiceResources.Get("ApiService.StartLog.Start"), callerMethodName);
        }

        /// <summary>
        /// Stops logging an action.
        /// </summary>
        /// <param name="message">Optional message to be logged.</param>
        protected void EndLog(string message = null)
        {
            _stopWatch.Stop();

            var callerMethodName = new StackTrace(1).GetMethodName();
            var totalSeconds = _stopWatch.Elapsed.TotalSeconds;

            Logger.LogDebug(ServiceResources.Get("ApiService.EndLog.End"), callerMethodName);
            Logger.LogDebug(ServiceResources.Get("ApiService.EndLog.TotalSeconds"), totalSeconds);

            if (!string.IsNullOrEmpty(message))
                Logger.LogDebug(message);
        }

        /// <summary>
        /// Validates the received payload using FluentValidation.
        /// </summary>
        /// <typeparam name="TPayload">The type of the payload object.
        /// </typeparam>
        /// <param name="payload">The object to be validated.</param>
        /// <param name="message">Message detailing the error in case
        /// the payload is invalid.</param>
        /// <returns>True if the payload is valid.</returns>
        protected bool Validate<TPayload>(TPayload payload, out string message)
        {
            if (payload == null)
                message = UtilResources.Get("Common.ArgumentCanNotBeNull", nameof(payload));
            else
            {
                var validator = ServiceProvider.GetRequiredService<IValidator<TPayload>>();
                var result = validator.Validate(payload);

                message = result.IsValid
                    ? null
                    : result.Errors[0].ErrorMessage;
            }

            return message == null;
        }

        /// <summary>
        /// Creates an Ok response based on the passed-in Data and the
        /// optional localized Message.
        /// </summary>
        /// <typeparam name="TResponse">The type to which this object
        /// will be mapped.</typeparam>
        /// <param name="data">Data sent as the response.</param>
        /// <param name="message">Optional localized Message.</param>
        /// <returns>Ok response based on the passed-in Data and the
        /// optional localized Message.</returns>
        protected ApiResponse<TResponse> Ok<TResponse>(object data, string message = null) =>
            new ApiResponse<TResponse>
            {
                Data = Mapper.Map<TResponse>(data),
                Message = string.IsNullOrEmpty(message)
                    ? null
                    : message,
                MessageType = string.IsNullOrEmpty(message)
                    ? (ApiMessageType?)null
                    : ApiMessageType.Success,
                Success = true
            };

        /// <summary>
        /// Creates an Ok response based on the passed-in Data and the
        /// operation type.
        /// </summary>
        /// <typeparam name="TResponse">The type to which this object
        /// will be mapped.</typeparam>
        /// <param name="data">Data sent as the response.</param>
        /// <param name="type">The type of operation.</param>
        /// <returns>Ok response based on the passed-in Data and the
        /// operation type.</returns>
        public ApiResponse<TResponse> Ok<TResponse>(object data, ApiOperationType type)
        {
            var message = (string)null;
            var messageType = (ApiMessageType?)null;

            switch (type)
            {
                case ApiOperationType.Read:
                    if (data is ICollection &&
                       (data as ICollection).Count == 0)
                    {
                        message = ServiceResources.Get("Common.NoRecordsFound");
                        messageType = ApiMessageType.Info;
                    }
                    break;
                case ApiOperationType.Insert:
                    message = ServiceResources.Get("ApiService.Response.Insert");
                    messageType = ApiMessageType.Success;
                    break;
                case ApiOperationType.BulkInsert:
                    message = ServiceResources.Get("ApiService.Response.BulkInsert");
                    messageType = ApiMessageType.Success;
                    break;
                case ApiOperationType.Update:
                    message = ServiceResources.Get("ApiService.Response.Update");
                    messageType = ApiMessageType.Success;
                    break;
                case ApiOperationType.BulkUpdate:
                    message = ServiceResources.Get("ApiService.Response.BulkUpdate");
                    messageType = ApiMessageType.Success;
                    break;
                case ApiOperationType.Delete:
                    message = ServiceResources.Get("ApiService.Response.Delete");
                    messageType = ApiMessageType.Success;
                    break;
                case ApiOperationType.BulkDelete:
                    message = ServiceResources.Get("ApiService.Response.BulkDelete");
                    messageType = ApiMessageType.Success;
                    break;
            }

            return new ApiResponse<TResponse>
            {
                Data = Mapper.Map<TResponse>(data),
                Message = message,
                MessageType = messageType,
                Success = true
            };
        }

        /// <summary>
        /// Creates an Error response based on the passed-in localized
        /// Message.
        /// </summary>
        /// <param name="message">Localized Message detailing the error.
        /// </param>
        /// <returns>Error response based on the passed-in localized
        /// Message.</returns>
        protected static ApiResponse Error(string message) =>
            new ApiResponse
            {
                Message = message,
                MessageType = ApiMessageType.Error,
                Success = false
            };

        /// <summary>
        /// Paginates this query according to the passed-in request params.
        /// </summary>
        /// <typeparam name="TRequest">The entity request type.</typeparam>
        /// <typeparam name="TResult">The entity result type.</typeparam>
        /// <param name="query">Object against we are querying.</param>
        /// <param name="request">Object with request arguments.</param>
        /// <returns>The task object representing the asynchronous operation.
        /// </returns>
        public async Task<ApiResponse<PaginationResult<TResult>>> PaginateAsync<TRequest, TResult>(
            IQueryable<TRequest> query,
            PaginationRequest request)
        {
            var pagination = await query.PaginateAsync(request);

            var castedPagination = new PaginationResult<TResult>
            {
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize,
                SortKey = pagination.SortKey,
                SortDirection = pagination.SortDirection,
                Items = Mapper.Map<IEnumerable<TResult>>(pagination.Items),
                RecordCount = pagination.RecordCount,
                PageCount = pagination.PageCount
            };

            ApiResponse<PaginationResult<TResult>> response;

            if (castedPagination.RecordCount == 0)
                response = new ApiResponse<PaginationResult<TResult>>
                {
                    Data = castedPagination,
                    Message = ServiceResources.Get("Common.NoRecordsFound"),
                    MessageType = ApiMessageType.Info,
                    Success = true
                };
            else
                response = Ok<PaginationResult<TResult>>(castedPagination);

            return response;
        }
    }
}
