﻿using Com.Atomatus.Bootstarter.Model;
using Com.Atomatus.Bootstarter.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace Com.Atomatus.Bootstarter.Web
{
    /// <summary>
    /// Versioned Controller to [C]reate and [R]ead Operations implementation for entity model using service.
    /// <para>
    /// This controller constains by default the following actions:<br/><br/>
    /// </para>
    /// 
    /// <para>
    /// ┌[C]reate:<br/>
    /// └─► <see cref="ControllerCr{TService, TModel, TID}.Create(TModel)"/>
    /// </para>
    /// 
    /// <para>
    /// ┌[R]ead:<br/>
    /// ├─► <see cref="ControllerCr{TService, TModel, TID}.Get()"/><br/>
    /// ├─► <see cref="ControllerCr{TService, TModel, TID}.Get(Guid)"/><br/>
    /// └─► <see cref="ControllerCr{TService, TModel, TID}.Get(TID)"/>
    /// </para>
    /// </summary>
    /// <typeparam name="TModel">entity model type</typeparam>
    /// <typeparam name="TID">entity model id type</typeparam>
    public abstract class ControllerCr<TModel, TID> : ControllerCr<IServiceCrud<TModel, TID>, TModel, TID>
        where TModel : IModel<TID>
    {
        /// <summary>
        /// Controller constructor with service data persistence and logging perform.<br/>
        /// The follow parameters can be set by dependency injection.
        /// </summary>
        /// <param name="service">service to data persistence</param>
        /// <param name="logger">logging target</param>
        protected ControllerCr(IServiceCrud<TModel, TID> service, ILogger<ControllerCr<TModel, TID>> logger) : base(service, logger) { }

        /// <summary>
        /// Controller constructor with service data persistence and logging perform.<br/>
        /// The follow parameters can be set by dependency injection.<br/>
        /// Using no logger performing.
        /// </summary>
        /// <param name="service">service to data persistence</param>
        protected ControllerCr(IServiceCrud<TModel, TID> service) : base(service) { }
    }

    /// <summary>
    /// Versioned Controller to [C]reate and [R]ead Operations implementation for entity model using service.
    /// <para>
    /// This controller constains by default the following actions:<br/><br/>
    /// </para>
    /// 
    /// <para>
    /// ┌[C]reate:<br/>
    /// └─► <see cref="Create(TModel)"/>
    /// </para>
    /// 
    /// <para>
    /// ┌[R]ead:<br/>
    /// ├─► <see cref="Get()"/><br/>
    /// ├─► <see cref="Get(Guid)"/><br/>
    /// └─► <see cref="Get(TID)"/>
    /// </para>
    /// </summary>
    /// <typeparam name="TService">target service to data persistence</typeparam>
    /// <typeparam name="TModel">entity model type</typeparam>
    /// <typeparam name="TID">entity model id type</typeparam>
    public abstract class ControllerCr<TService, TModel, TID> : ControllerCrudBase<TService, TModel, TID>
        where TService : IServiceCrud<TModel, TID>
        where TModel : IModel<TID>
    {
        /// <summary>
        /// Controller constructor with service data persistence and logging perform.<br/>
        /// The follow parameters can be set by dependency injection.
        /// </summary>
        /// <param name="service">service to data persistence</param>
        /// <param name="logger">logging target</param>
        protected ControllerCr(TService service, ILogger<ControllerCr<TService, TModel, TID>> logger) : base(service, logger) { }

        /// <summary>
        /// Controller constructor with service data persistence and logging perform.<br/>
        /// The follow parameters can be set by dependency injection.<br/>
        /// Using no logger performing.
        /// </summary>
        /// <param name="service">service to data persistence</param>
        protected ControllerCr(TService service) : base(service) { }

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
        /// <param name="result">model from body</param>
        /// <returns>action result</returns>        
        [HttpPost]
        public virtual IActionResult Create([FromBody] TModel result) => CreateAction(result);
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
        /// </summary>
        /// <returns>action result</returns>    
        [HttpGet]
        public virtual IActionResult Get() => GetAction();

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
        /// <returns>action result</returns>    
        [HttpGet("{id}")]
        public virtual IActionResult Get(TID id) => GetAction(id);

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
        /// <returns>action result</returns>    
        [HttpGet("uuid/{uuid}")]
        public virtual IActionResult Get(Guid uuid) => GetAction(uuid);

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
        /// <param name="page">page index, from 0</param>
        /// <param name="limit">page limit request</param>
        /// <returns>action result</returns>    
        [HttpGet("page/{page}/{limit:int?}")]
        public virtual IActionResult Paging(int page, int limit = -1) => PagingAction(page, limit);
        #endregion
    }
}
