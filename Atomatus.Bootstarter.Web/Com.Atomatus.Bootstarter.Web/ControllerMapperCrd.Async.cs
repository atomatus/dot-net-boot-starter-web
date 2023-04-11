using Com.Atomatus.Bootstarter.Model;
using Com.Atomatus.Bootstarter.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Com.Atomatus.Bootstarter.Web
{
    /// <summary>
    /// Versioned Controller [C]reate, [R]ead and [D]elete operation implementation for entity model protected by DTO pattern using service.
    /// <para>
    /// This controller constains by default the following actions:<br/><br/>
    /// </para>
    /// 
    /// <para>
    /// ┌[C]reate:<br/>
    /// └─► <see cref="ControllerMapperCrdAsync{TService, TModel, TDtoIn, TDtoOut}.CreateAsync(TDtoIn)"/>
    /// </para>
    /// 
    /// <para>
    /// ┌[R]ead:<br/>
    /// ├─► <see cref="ControllerMapperCrdAsync{TService, TModel, TDtoIn, TDtoOut}.GetAsync(CancellationToken)"/><br/>
    /// ├─► <see cref="ControllerMapperCrdAsync{TService, TModel, TDtoIn, TDtoOut}.GetAsync(Guid)"/><br/>
    /// └─► <see cref="ControllerMapperCrdAsync{TService, TModel, TDtoIn, TDtoOut}.PagingAsync(int, int, CancellationToken)"/>
    /// </para>
    /// 
    /// <para>
    /// ┌[D]elete:<br/>
    /// └─► <see cref="ControllerMapperCrdAsync{TService, TModel, TDtoIn, TDtoOut}.DeleteAsync(Guid)"/>
    /// </para>
    /// </summary>
    /// <typeparam name="TModel">entity model type</typeparam>
    /// <typeparam name="TDtoIn">dto input type, accepted in creation action body</typeparam>
    /// <typeparam name="TDtoOut">dto output type, generated after creation or read operation successfully</typeparam>
    public abstract class ControllerMapperCrdAsync<TModel, TDtoIn, TDtoOut> : ControllerMapperCrdAsync<IServiceCrudAsync<TModel>, TModel, TDtoIn, TDtoOut>
        where TModel : class, IModel, new()
        where TDtoIn : class, new()
        where TDtoOut : class, new()
    {
        /// <summary>
        /// Controller CRUD constructor with service data persistence and logging perform.<br/>
        /// The follow parameters can be set by dependency injection.
        /// </summary>
        /// <param name="service">service to data persistence</param>
        /// <param name="logger">logging target</param>
        protected ControllerMapperCrdAsync(IServiceCrudAsync<TModel> service, ILogger<ControllerMapperCrdAsync<TModel, TDtoIn, TDtoOut>> logger) : base(service, logger) { }

        /// <summary>
        /// Controller CRUD constructor with service data persistence and logging perform.<br/>
        /// The follow parameters can be set by dependency injection.<br/>
        /// Using no logger performing.
        /// </summary>
        /// <param name="service">service to data persistence</param>
        protected ControllerMapperCrdAsync(IServiceCrudAsync<TModel> service) : base(service) { }
    }

    /// <summary>
    /// Versioned Controller [C]reate, [R]ead, [U]pdate and [D]elete operation implementation for entity model protected by DTO pattern and using service.
    /// <para>
    /// This controller constains by default the following actions:<br/><br/>
    /// </para>
    /// 
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
    /// </para>
    /// 
    /// <para>
    /// ┌[D]elete:<br/>
    /// └─► <see cref="DeleteAsync(Guid)"/>
    /// </para>
    /// </summary>
    /// <typeparam name="TService">target service to data persistence</typeparam>
    /// <typeparam name="TModel">entity model type</typeparam>
    /// <typeparam name="TDtoIn">dto input type, accepted in creation action body</typeparam>
    /// <typeparam name="TDtoOut">dto output type, generated after creation or read operation successfully</typeparam>
    public abstract class ControllerMapperCrdAsync<TService, TModel, TDtoIn, TDtoOut> : ControllerMapperCrudBaseAsync<TService, TModel>
        where TService : IServiceCrudAsync<TModel>
        where TModel : class, IModel, new()
        where TDtoIn : class, new()
        where TDtoOut : class, new()
    {
        /// <summary>
        /// Controller CRUD constructor with service data persistence and logging perform.<br/>
        /// The follow parameters can be set by dependency injection.
        /// </summary>
        /// <param name="service">service to data persistence</param>
        /// <param name="logger">logging target</param>
        protected ControllerMapperCrdAsync(TService service, ILogger<ControllerMapperCrdAsync<TService, TModel, TDtoIn, TDtoOut>> logger) : base(service, logger) { }

        /// <summary>
        /// Controller CRUD constructor with service data persistence and logging perform.<br/>
        /// The follow parameters can be set by dependency injection.<br/>
        /// Using no logger performing.
        /// </summary>
        /// <param name="service">service to data persistence</param>
        protected ControllerMapperCrdAsync(TService service) : base(service) { }

        #region [C]reate
        /// <summary>
        /// <para>Perform a write operation to persist data.</para>
        /// <i>https://api.urladdress/v1 (POST Method/ DTO data from body)</i>
        /// <para>
        /// Results<br/>
        /// ● OK: Successfully, contains DTO with Uuid.<br/>
        /// ● Bad Request: Aleady exists or some another error.
        /// </para>
        /// </summary>
        /// <param name="result">DTO from body (<typeparamref name="TDtoIn"/>)</param>
        /// <returns>action result (<typeparamref name="TDtoOut"/>)</returns>        
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
        /// <returns>action result (<typeparamref name="TDtoIn"/>)</returns>    
        [HttpGet]
        public virtual Task<IActionResult> GetAsync(CancellationToken cancellationToken) => GetActionAsync<TDtoOut>(cancellationToken);

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
        /// <returns>action result (<typeparamref name="TDtoIn"/>)</returns>    
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
        /// <returns>action result (<typeparamref name="TDtoIn"/>)</returns>    
        [HttpGet("page/{page}/{limit:int?}")]
        public virtual Task<IActionResult> PagingAsync(int page, int limit = -1, CancellationToken cancellationToken = default) => PagingActionAsync<TDtoOut>(page, limit, cancellationToken);
        #endregion

        #region [D]elete
        /// <summary>
        /// <para>Perform a write operation to update data.</para>
        /// <i>https://api.urladdress/v1/{uuid} (DELETE Method)</i>
        /// <para>
        /// Results<br/>
        /// ● OK: Successfully, data deleted.<br/>
        /// ● Not Found: target data does not exists.<br/>
        /// ● Bad Request: some error, invalid UUID or some internal error.
        /// </para>
        /// </summary>
        /// <param name="uuid">target uuid entity</param>
        /// <returns>action result (<typeparamref name="TDtoIn"/>)</returns>        
        [HttpDelete("{uuid}")]
        public virtual Task<IActionResult> DeleteAsync(Guid uuid) => DeleteActionAsync(uuid);
        #endregion
    }

}
