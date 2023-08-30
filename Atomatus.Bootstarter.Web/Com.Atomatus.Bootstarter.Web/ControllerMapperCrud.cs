using Com.Atomatus.Bootstarter.Model;
using Com.Atomatus.Bootstarter.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace Com.Atomatus.Bootstarter.Web
{
    /// <summary>
    /// Versioned Controller CRUD ([C]reate, [R]ead, [U]pdate and [D]elete) operation implementation for entity model protected by DTO pattern using service.
    /// <para>
    /// This controller constains by default the following actions:<br/><br/>
    /// </para>
    /// 
    /// <para>
    /// ┌[C]reate:<br/>
    /// └─► <see cref="ControllerMapperCrud{TService, TModel, TDtoIn, TDtoOut}.Create(TDtoIn)"/>
    /// </para>
    /// 
    /// <para>
    /// ┌[R]ead:<br/>
    /// ├─► <see cref="ControllerMapperCrud{TService, TModel, TDtoIn, TDtoOut}.Get()"/><br/>
    /// ├─► <see cref="ControllerMapperCrud{TService, TModel, TDtoIn, TDtoOut}.Get(Guid)"/><br/>
    /// └─► <see cref="ControllerMapperCrud{TService, TModel, TDtoIn, TDtoOut}.Paging(int, int)"/>
    /// </para>
    /// 
    /// <para>
    /// ┌[U]pdate:<br/>
    /// └─► <see cref="ControllerMapperCrud{TService, TModel, TDtoIn, TDtoOut}.Update(TDtoIn)"/>
    /// </para>
    /// 
    /// <para>
    /// ┌[D]elete:<br/>
    /// └─► <see cref="ControllerMapperCrud{TService, TModel, TDtoIn, TDtoOut}.Delete(Guid)"/>
    /// </para>
    /// </summary>
    /// <typeparam name="TModel">entity model type</typeparam>
    /// <typeparam name="TDtoIn">dto input type, accepted in creation action body</typeparam>
    /// <typeparam name="TDtoOut">dto output type, generated after creation or read operation successfully</typeparam>
    public abstract class ControllerMapperCrud<TModel, TDtoIn, TDtoOut> : ControllerMapperCrud<IServiceCrud<TModel>, TModel, TDtoIn, TDtoOut>
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
        protected ControllerMapperCrud(IServiceCrud<TModel> service, ILogger<ControllerMapperCrud<TModel, TDtoIn, TDtoOut>> logger) : base(service, logger) { }

        /// <summary>
        /// Controller CRUD constructor with service data persistence and logging perform.<br/>
        /// The follow parameters can be set by dependency injection.<br/>
        /// Using no logger performing.
        /// </summary>
        /// <param name="service">service to data persistence</param>
        protected ControllerMapperCrud(IServiceCrud<TModel> service) : base(service) { }
    }

    /// <summary>
    /// Versioned Controller CRUD ([C]reate, [R]ead, [U]pdate and [D]elete) operation implementation for entity model protected by DTO pattern and using service.
    /// <para>
    /// This controller constains by default the following actions:<br/><br/>
    /// </para>
    /// 
    /// <para>
    /// ┌[C]reate:<br/>
    /// └─► <see cref="Create(TDtoIn)"/>
    /// </para>
    /// 
    /// <para>
    /// ┌[R]ead:<br/>
    /// ├─► <see cref="Get()"/><br/>
    /// ├─► <see cref="Get(Guid)"/><br/>
    /// └─► <see cref="Paging(int, int)"/>
    /// </para>
    /// 
    /// <para>
    /// ┌[U]pdate:<br/>
    /// └─► <see cref="Update(TDtoIn)"/>
    /// </para>
    /// 
    /// <para>
    /// ┌[D]elete:<br/>
    /// └─► <see cref="Delete(Guid)"/>
    /// </para>
    /// </summary>
    /// <typeparam name="TService">target service to data persistence</typeparam>
    /// <typeparam name="TModel">entity model type</typeparam>
    /// <typeparam name="TDtoIn">dto input type, accepted in creation action body</typeparam>
    /// <typeparam name="TDtoOut">dto output type, generated after creation or read operation successfully</typeparam>
    public abstract class ControllerMapperCrud<TService, TModel, TDtoIn, TDtoOut> : ControllerMapperCrudBase<TService, TModel>
        where TService : IServiceCrud<TModel>
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
        protected ControllerMapperCrud(TService service, ILogger<ControllerMapperCrud<TService, TModel, TDtoIn, TDtoOut>> logger) : base(service, logger) { }

        /// <summary>
        /// Controller CRUD constructor with service data persistence and logging perform.<br/>
        /// The follow parameters can be set by dependency injection.<br/>
        /// Using no logger performing.
        /// </summary>
        /// <param name="service">service to data persistence</param>
        protected ControllerMapperCrud(TService service) : base(service) { }

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
        public virtual IActionResult Create([FromBody] TDtoIn result) => CreateAction<TDtoIn, TDtoOut>(result);
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
        /// <returns>action result (<typeparamref name="TDtoIn"/>)</returns>    
        [HttpGet]
        public virtual IActionResult Get() => GetAction<TDtoOut>();

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
        public virtual IActionResult Get(Guid uuid) => GetAction<TDtoOut>(uuid);

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
        /// <returns>action result (<typeparamref name="TDtoIn"/>)</returns>    
        [HttpGet("page/{page}/{limit:int?}")]
        public virtual IActionResult Paging(int page, int limit = -1) => PagingAction<TDtoOut>(page, limit);
        #endregion

        #region [U]pdate
        /// <summary>
        /// <para>Perform a write operation to update data.</para>
        /// <i>https://api.urladdress/v1 (PUT Method/ DTO data from body)</i>
        /// <para>
        /// Results<br/>
        /// ● OK: Successfully, data updated.<br/>
        /// ● Not Found: target data does not exists.<br/>
        /// ● Bad Request: some error.
        /// </para>
        /// </summary>
        /// <param name="result">DTO from body (<typeparamref name="TDtoIn"/>)</param>
        /// <returns>action result (<typeparamref name="TDtoOut"/>)</returns>        
        [HttpPut]
        public virtual IActionResult Update([FromBody] TDtoIn result) => UpdateAction(result);
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
        public virtual IActionResult Delete(Guid uuid) => DeleteAction(uuid);
        #endregion
    }

}
