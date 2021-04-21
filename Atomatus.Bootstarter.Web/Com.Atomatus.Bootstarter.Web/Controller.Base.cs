using Com.Atomatus.Bootstarter.Model;
using Com.Atomatus.Bootstarter.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace Com.Atomatus.Bootstarter.Web
{
    /// <summary>
    /// API Controller base implementing
    /// the default route "api/v{version:apiVersion}/[controller]" 
    /// and produces an "application/json" as default.
    /// </summary>
    /// <typeparam name="TService">target service type to data persistence</typeparam>
    /// <typeparam name="TModel">target model type</typeparam>    
    [ApiController]
    [Produces("application/json")]
    [Route("v{version:apiVersion}/[controller]")]
    public abstract class ControllerBase<TService, TModel> : ControllerBase
        where TService : IService
        where TModel : IModel
    {
        private class EmptyLogger : ILogger<ControllerBase<TService, TModel>>, IDisposable
        {
            public IDisposable BeginScope<TState>(TState state)
            {
                return this;
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return false;
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) { }

            void IDisposable.Dispose() { }
        }

        /// <summary>
        /// Service for controller.
        /// </summary>
        internal protected readonly TService service;

        /// <summary>
        /// Logger for controller.
        /// </summary>
        internal protected readonly ILogger logger;
        
        /// <summary>
        /// Constroller constructor.
        /// The follow parameters can be set by dependency injection.
        /// </summary>
        /// <param name="service">service target</param>
        /// <param name="logger">logger targer</param>
        protected ControllerBase(TService service, ILogger<ControllerBase<TService, TModel>> logger)
        {
            this.service = service ?? 
                throw new ArgumentException($"The Service \"{typeof(TService).Name}\" ({typeof(TService).FullName}) " +
                $"is not defined. Verify if this service was defined in services collection provider!");

            this.logger = logger ??
                throw new ArgumentException($"The ILogger is not defined. " +
                $"Verify if this logger service was defined in services collection provider!");
        }

        /// <summary>
        /// Constroller constructor.
        /// The follow parameters can be set by dependency injection.
        /// Using no logger performing.
        /// </summary>
        /// <param name="service">service target</param>
        protected ControllerBase(TService service) : this(service, new EmptyLogger()) { }

        /// <summary>
        /// Request to check if current id is valid.
        /// </summary>
        /// <typeparam name="ID">id type</typeparam>
        /// <param name="id">target id</param>
        /// <returns>true, id valid, otherwise, false.</returns>
        protected virtual bool IsValidId<ID>(ID id)
        {
            return
                id is short s ? s > 0 :
                id is int i ? i > 0 :
                id is long l ? l > 0L :
                id is string str ? !string.IsNullOrWhiteSpace(str) :
                id is Guid guid && guid != default;
        }

        /// <summary>
        /// Request valid id or throws exception.
        /// </summary>
        /// <typeparam name="ID">id type</typeparam>
        /// <param name="id">target id</param>
        /// <exception cref="ArgumentException">throws when id is invalid</exception>
        protected virtual void RequireValidId<ID>(ID id)
        {
            if (!IsValidId(id))
            {
                throw new ArgumentException("Invalid id!");
            }
        }

    }
}
