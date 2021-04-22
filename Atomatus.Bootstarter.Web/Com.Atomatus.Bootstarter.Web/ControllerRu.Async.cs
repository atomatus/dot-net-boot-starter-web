﻿using Com.Atomatus.Bootstarter.Model;
using Com.Atomatus.Bootstarter.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Com.Atomatus.Bootstarter.Web
{
    /// <summary>
    /// Versioned Controller [R]ead and [U]pdate async operation implementation for entity model using service.
    /// <para>
    /// This controller constains by default the following actions:<br/><br/>
    /// </para>
    /// <para>
    /// ┌[R]ead:<br/>
    /// ├─► <see cref="ControllerRuAsync{TService, TModel, TID}.GetAsync(CancellationToken)"/><br/>
    /// ├─► <see cref="ControllerRuAsync{TService, TModel, TID}.GetAsync(Guid)"/><br/>
    /// └─► <see cref="ControllerRuAsync{TService, TModel, TID}.GetAsync(TID)"/>
    /// </para>
    /// 
    /// <para>
    /// ┌[U]pdate:<br/>
    /// └─► <see cref="ControllerRuAsync{TService, TModel, TID}.UpdateAsync(TModel)"/>
    /// </para>
    /// 
    /// </summary>
    /// <typeparam name="TModel">entity model type</typeparam>
    /// <typeparam name="TID">entity model id type</typeparam>
    public abstract class ControllerRuAsync<TModel, TID> : ControllerRuAsync<IServiceCrudAsync<TModel, TID>, TModel, TID>
        where TModel : IModel<TID>
    {
        /// <summary>
        /// Controller constructor with service data persistence and logging perform.<br/>
        /// The follow parameters can be set by dependency injection.
        /// </summary>
        /// <param name="service">service to data persistence</param>
        /// <param name="logger">logging target</param>
        protected ControllerRuAsync(IServiceCrudAsync<TModel, TID> service, ILogger<ControllerRuAsync<TModel, TID>> logger) : base(service, logger) { }

        /// <summary>
        /// Controller constructor with service data persistence and logging perform.<br/>
        /// The follow parameters can be set by dependency injection.<br/>
        /// Using no logger performing.
        /// </summary>
        /// <param name="service">service to data persistence</param>
        protected ControllerRuAsync(IServiceCrudAsync<TModel, TID> service) : base(service) { }
    }

    /// <summary>
    /// Versioned Controller [R]ead and [U]pdate async operation implementation for entity model using service.
    /// <para>
    /// This controller constains by default the following actions:<br/><br/>
    /// </para>
    /// <para>
    /// ┌[R]ead:<br/>
    /// ├─► <see cref="GetAsync(CancellationToken)"/><br/>
    /// ├─► <see cref="GetAsync(Guid)"/><br/>
    /// └─► <see cref="GetAsync(TID)"/>
    /// </para>
    /// 
    /// <para>
    /// ┌[U]pdate:<br/>
    /// └─► <see cref="UpdateAsync(TModel)"/>
    /// </para>
    /// 
    /// </summary>
    /// <typeparam name="TService">target service to data persistence</typeparam>
    /// <typeparam name="TModel">entity model type</typeparam>
    /// <typeparam name="TID">entity model id type</typeparam>
    public abstract class ControllerRuAsync<TService, TModel, TID> : ControllerCrudBaseAsync<TService, TModel, TID>
        where TService : IServiceCrudAsync<TModel, TID>
        where TModel : IModel<TID>
    {
        /// <summary>
        /// Controller constructor with service data persistence and logging perform.<br/>
        /// The follow parameters can be set by dependency injection.
        /// </summary>
        /// <param name="service">service to data persistence</param>
        /// <param name="logger">logging target</param>
        protected ControllerRuAsync(TService service, ILogger<ControllerRuAsync<TService, TModel, TID>> logger) : base(service, logger) { }

        /// <summary>
        /// Controller constructor with service data persistence and logging perform.<br/>
        /// The follow parameters can be set by dependency injection.<br/>
        /// Using no logger performing.
        /// </summary>
        /// <param name="service">service to data persistence</param>
        protected ControllerRuAsync(TService service) : base(service) { }

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
        /// <returns>action result task</returns>    
        [HttpGet]
        public virtual Task<IActionResult> GetAsync(CancellationToken cancellationToken) => GetActionAsync(cancellationToken);

        /// <summary>
        /// <para>Perform a request operation to find register by id.</para>
        /// <i>https://api.urladdress/v1/{id} (GET Method)</i>
        /// <para>
        /// Results<br/>
        /// ● OK: Successfully, contains result.<br/>
        /// ● Not Found: Does not exists register with id.<br/>
        /// ● Bad Request: some error in request.
        /// </para>
        /// </summary>
        /// <param name="id">targer id</param>
        /// <returns>action result task</returns>    
        [HttpGet("{id}")]
        public virtual Task<IActionResult> GetAsync(TID id) => GetActionAsync(id);

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
        /// <returns>action result task</returns>        
        [HttpGet("uuid/{uuid}")]
        public virtual Task<IActionResult> GetAsync(Guid uuid) => GetActionAsync(uuid);

        /// <summary>
        /// <para>Perform a request operation to find registers by paging.</para>
        /// <i>https://api.urladdress/v1/page/{page}/{limit} (GET Method)</i><br/>
        /// <i>https://api.urladdress/v1/page/{page} (GET Method, using default limit request of 300)</i>
        /// <para>
        /// Results<br/>
        /// ● OK: Successfully, contains result or empty result.<br/>
        /// ● Bad Request: some error in request.
        /// </para>
        /// </summary>
        /// <i> This operation can be cancelled.</i>
        /// <param name="page">page index, from 0</param>
        /// <param name="limit">page limit request</param>
        /// <param name="cancellationToken">cancellation token</param>
        /// <returns>action result task</returns>    
        [HttpGet("page/{page}/{limit:int?}")]
        public virtual Task<IActionResult> PagingAsync(int page, int limit = -1, CancellationToken cancellationToken = default)
            => PagingActionAsync(page, limit, cancellationToken);
        #endregion
        
        #region [U]pdate
        /// <summary>
        /// <para>Perform a write operation to update data.</para>
        /// <i>https://api.urladdress/v1 (PUT Method/ Model data from body)</i>
        /// <para>
        /// Results<br/>
        /// ● OK: Successfully, data updated.<br/>
        /// ● Not Found: target data does not exists.<br/>
        /// ● Bad Request: some error.
        /// </para>
        /// </summary>
        /// <param name="result">model from body</param>
        /// <returns>action result task</returns>        
        [HttpPut]
        public virtual Task<IActionResult> UpdateAsync([FromBody] TModel result) => UpdateActionAsync(result);
        #endregion
    }
}
