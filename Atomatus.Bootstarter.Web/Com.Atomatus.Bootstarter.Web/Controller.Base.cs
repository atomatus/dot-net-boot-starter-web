using Com.Atomatus.Bootstarter.Model;
using Com.Atomatus.Bootstarter.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace Com.Atomatus.Bootstarter.Web
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public abstract class ControllerBase<Service, Model> : ControllerBase
        where Service : IService
        where Model : IModel
    {
        private class EmptyLogger : ILogger<ControllerBase<Service, Model>>, IDisposable
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

        protected readonly Service service;
        protected readonly ILogger logger;
        
        protected ControllerBase(Service service, ILogger<ControllerBase<Service, Model>> logger)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected ControllerBase(Service service) : this(service, new EmptyLogger()) { }

        protected virtual bool IsValidId<ID>(ID id)
        {
            return
                id is short s ? s > 0 :
                id is int i ? i > 0 :
                id is long l ? l > 0L :
                id is string str ? !string.IsNullOrWhiteSpace(str) :
                id is Guid guid && guid != default;
        }

        protected virtual void RequireValidId<ID>(ID id)
        {
            if (!IsValidId(id))
            {
                throw new ArgumentException("Invalid id!");
            }
        }

    }
}
