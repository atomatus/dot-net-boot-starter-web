using System;
using System.Threading;
using System.Threading.Tasks;
using Com.Atomatus.Bootstarter.Model;
using Com.Atomatus.Bootstarter.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Com.Atomatus.Bootstarter.Web
{
    /// <summary>
    /// Versioned Controller [C]reate and [R]ead Operations implementation for entity model protected by DTO patterns and using service.
    /// <para>
    /// This controller constains by default the following actions:<br/><br/>
    /// </para>
    /// <para>
    /// ┌[C]reate:<br/>
    /// └─► <see cref="ControllerMapperCrAsync{TService, TModel, TDtoIn, TDtoOut}.CreateAsync(TDtoIn)"/>
    /// </para>
    /// 
    /// <para>
    /// ┌[R]ead:<br/>
    /// ├─► <see cref="ControllerMapperCrAsync{TService, TModel, TDtoIn, TDtoOut}.GetAsync(CancellationToken)"/><br/>
    /// ├─► <see cref="ControllerMapperCrAsync{TService, TModel, TDtoIn, TDtoOut}.GetAsync(Guid)"/><br/>
    /// └─► <see cref="ControllerMapperCrAsync{TService, TModel, TDtoIn, TDtoOut}.PagingAsync(int, int, CancellationToken)"/>
    /// </para>
    /// </summary>
    /// <typeparam name="TModel">entity model type</typeparam>
    /// <typeparam name="TDtoIn">dto input type, accepted in creation action body</typeparam>
    /// <typeparam name="TDtoOut">dto output type, generated after creation or read operation successfully</typeparam>
    public abstract class ControllerMapperCrAsync<TModel, TDtoIn, TDtoOut> : ControllerMapperCrAsync<IServiceCrudAsync<TModel>, TModel, TDtoIn, TDtoOut>
        where TModel : class, IModel, new()
        where TDtoIn : class, new()
        where TDtoOut : class, new()
    {
        /// <summary>
        /// Controller constructor with service data persistence and logging perform.<br/>
        /// The follow parameters can be set by dependency injection.
        /// </summary>
        /// <param name="service">service to data persistence</param>
        /// <param name="logger">logging target</param>
        protected ControllerMapperCrAsync(IServiceCrudAsync<TModel> service, ILogger<ControllerMapperCrAsync<TModel, TDtoIn, TDtoOut>> logger) : base(service, logger) { }

        /// <summary>
        /// Controller constructor with service data persistence and logging perform.<br/>
        /// The follow parameters can be set by dependency injection.<br/>
        /// Using no logger performing.
        /// </summary>
        /// <param name="service">service to data persistence</param>
        protected ControllerMapperCrAsync(IServiceCrudAsync<TModel> service) : base(service) { }
    }

    /// <summary>
    /// Versioned Controller [C]reate Operation implementation for entity model using service.
    /// <para>
    /// This controller constains by default the following actions:<br/><br/>
    /// </para>
    /// <para>
    /// ┌[C]reate:<br/>
    /// └─► <see cref="CreateAsync(TDtoIn)"/>
    /// </para>
    /// 
    /// <para>
    /// ┌[R]ead:<br/>
    /// ├─► <see cref="GetAsync(CancellationToken)"/><br/>
    /// ├─► <see cref="GetAsync(Guid)"/><br/>
    /// └─► <see cref="PagingAsync(int, int, CancellationToken)"/>
    /// </summary>
    /// <typeparam name="TService">target service to data persistence</typeparam>
    /// <typeparam name="TModel">entity model type</typeparam>
    /// <typeparam name="TDtoIn">dto input type, accepted in creation action body</typeparam>
    /// <typeparam name="TDtoOut">dto output type, generated after creation or read operation successfully</typeparam>
    public abstract class ControllerMapperCrAsync<TService, TModel, TDtoIn, TDtoOut> : ControllerMapperCrudBaseAsync<TService, TModel>
        where TService : IServiceCrudAsync<TModel>
        where TModel : class, IModel, new()
        where TDtoIn : class, new()
        where TDtoOut : class, new()
    {
        /// <summary>
        /// Controller constructor with service data persistence and logging perform.<br/>
        /// The follow parameters can be set by dependency injection.
        /// </summary>
        /// <param name="service">service to data persistence</param>
        /// <param name="logger">logging target</param>
        protected ControllerMapperCrAsync(TService service, ILogger<ControllerMapperCrAsync<TService, TModel, TDtoIn, TDtoOut>> logger) : base(service, logger) { }

        /// <summary>
        /// Controller constructor with service data persistence and logging perform.<br/>
        /// The follow parameters can be set by dependency injection.<br/>
        /// Using no logger performing.
        /// </summary>
        /// <param name="service">service to data persistence</param>
        protected ControllerMapperCrAsync(TService service) : base(service) { }

        #region [C]reate
        /// <summary>
        /// <para>Perform a write operation to persist data.</para>
        /// <i>https://api.urladdress/v1 (POST Method/ Model data from body)</i>
        /// <para>
        /// Results<br/>
        /// ● OK: Successfully, contains model with Uuid.<br/>
        /// ● Bad Request: Aleady exists or some another error.
        /// </para>
        /// </summary>
        /// <param name="result">dto input from body (<typeparamref name="TDtoIn"/>)</param>
        /// <returns>action result (dto output <typeparamref name="TDtoOut"/>)</returns>        
        [HttpPost]
        public virtual Task<IActionResult> CreateAsync([FromBody] TDtoIn result) => CreateActionAsync<TDtoIn, TDtoOut>(result);
        #endregion

        #region [R]ead
        /// <summary>
        /// <para>
        /// Perform a request operation to find all registers (limited to max request in service).
        /// </para>
        /// <i>https://api.urladdress/v1 (GET Method)</i>
        /// <para>
        /// Results<br/>
        /// ● OK: Successfully, contains result list.<br/>
        /// ● Bad Request: some error in request.
        /// </para>
        /// <i> This operation can be cancelled.</i>
        /// </summary>
        /// <param name="cancellationToken">cancellation token</param>
        /// <returns>action result (dto output <typeparamref name="TDtoOut"/>)</returns>   
        [HttpGet]
        public virtual Task<IActionResult> GetAsync(CancellationToken cancellationToken = default) => GetActionAsync<TDtoOut>(cancellationToken);

        /// <summary>
        /// <para>Perform a request operation to find register by uuid.</para>
        /// <i>https://api.urladdress/v1/uuid/{uuid} (GET Method)</i>
        /// <para>
        /// Results<br/>
        /// ● OK: Successfully, contains result.<br/>
        /// ● Not Found: Does not exists register with uuid.<br/>
        /// ● Bad Request: some error in request.
        /// </para>
        /// </summary>
        /// <param name="uuid">targer uuid</param>
        /// <returns>action result (dto output <typeparamref name="TDtoOut"/>)</returns>    
        [HttpGet("uuid/{uuid}")]
        public virtual Task<IActionResult> GetAsync(Guid uuid) => GetActionAsync<TDtoOut>(uuid);

        /// <summary>
        /// <para>Perform a request operation to find registers by paging.</para>
        /// <i>https://api.urladdress/v1/page/{page}/{limit} (GET Method)</i><br/>
        /// <i>https://api.urladdress/v1/page/{page} (GET Method, using default limit request of 300)</i>
        /// <para>
        /// Results<br/>
        /// ● OK: Successfully, contains result or empty result.<br/>
        /// ● Bad Request: some error in request.
        /// </para>
        /// <i> This operation can be cancelled.</i>
        /// </summary>
        /// <param name="page">page index, from 0</param>
        /// <param name="limit">page limit request</param>
        /// <param name="cancellationToken">cancellation token</param>
        /// <returns>action result (dto output <typeparamref name="TDtoOut"/>)</returns>   
        [HttpGet("page/{page}/{limit:int?}")]
        public virtual Task<IActionResult> PagingAsync(int page, int limit = -1, CancellationToken cancellationToken = default) => PagingActionAsync<TDtoOut>(page, limit, cancellationToken);
        #endregion
    }
}
