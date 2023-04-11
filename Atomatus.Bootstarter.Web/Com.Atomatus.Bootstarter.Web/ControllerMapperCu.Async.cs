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
    /// Versioned Controller [C]reate and [U]pdate operation implementation for entity model protected by DTO pattern using service.
    /// <para>
    /// This controller constains by default the following actions:<br/><br/>
    /// </para>
    /// 
    /// <para>
    /// ┌[C]reate:<br/>
    /// └─► <see cref="ControllerMapperCuAsync{TService, TModel, TDtoIn, TDtoOut}.CreateAsync(TDtoIn)"/>
    /// </para>
    /// 
    /// <para>
    /// ┌[U]pdate:<br/>
    /// └─► <see cref="ControllerMapperCuAsync{TService, TModel, TDtoIn, TDtoOut}.UpdateAsync(TDtoIn)"/>
    /// </para>
    /// 
    /// </summary>
    /// <typeparam name="TModel">entity model type</typeparam>
    /// <typeparam name="TDtoIn">dto input type, accepted in creation action body</typeparam>
    /// <typeparam name="TDtoOut">dto output type, generated after creation or read operation successfully</typeparam>
    public abstract class ControllerMapperCuAsync<TModel, TDtoIn, TDtoOut> : ControllerMapperCuAsync<IServiceCrudAsync<TModel>, TModel, TDtoIn, TDtoOut>
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
        protected ControllerMapperCuAsync(IServiceCrudAsync<TModel> service, ILogger<ControllerMapperCuAsync<TModel, TDtoIn, TDtoOut>> logger) : base(service, logger) { }

        /// <summary>
        /// Controller CRUD constructor with service data persistence and logging perform.<br/>
        /// The follow parameters can be set by dependency injection.<br/>
        /// Using no logger performing.
        /// </summary>
        /// <param name="service">service to data persistence</param>
        protected ControllerMapperCuAsync(IServiceCrudAsync<TModel> service) : base(service) { }
    }

    /// <summary>
    /// Versioned Controller [C]reate and [U]pdate operation implementation for entity model protected by DTO pattern and using service.
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
    /// ┌[U]pdate:<br/>
    /// └─► <see cref="UpdateAsync(TDtoIn)"/>
    /// </para>
    /// 
    /// </summary>
    /// <typeparam name="TService">target service to data persistence</typeparam>
    /// <typeparam name="TModel">entity model type</typeparam>
    /// <typeparam name="TDtoIn">dto input type, accepted in creation action body</typeparam>
    /// <typeparam name="TDtoOut">dto output type, generated after creation or read operation successfully</typeparam>
    public abstract class ControllerMapperCuAsync<TService, TModel, TDtoIn, TDtoOut> : ControllerMapperCrudBaseAsync<TService, TModel>
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
        protected ControllerMapperCuAsync(TService service, ILogger<ControllerMapperCuAsync<TService, TModel, TDtoIn, TDtoOut>> logger) : base(service, logger) { }

        /// <summary>
        /// Controller CRUD constructor with service data persistence and logging perform.<br/>
        /// The follow parameters can be set by dependency injection.<br/>
        /// Using no logger performing.
        /// </summary>
        /// <param name="service">service to data persistence</param>
        protected ControllerMapperCuAsync(TService service) : base(service) { }

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
        public virtual Task<IActionResult> UpdateAsync([FromBody] TDtoIn result) => UpdateActionAsync(result);
        #endregion

    }

}
