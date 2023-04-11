using System.Threading.Tasks;
using Com.Atomatus.Bootstarter.Model;
using Com.Atomatus.Bootstarter.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Com.Atomatus.Bootstarter.Web
{
    /// <summary>
    /// Versioned Controller [C]reate Operation implementation for entity model protected by DTO patterns and using service.
    /// <para>
    /// This controller constains by default the following actions:<br/><br/>
    /// </para>
    /// <para>
    /// ┌[C]reate:<br/>
    /// └─► <see cref="ControllerMapperCAsync{TService, TModel, TDtoIn, TDtoOut}.Create(TDtoIn)"/>
    /// </para>
    /// </summary>
    /// <typeparam name="TModel">entity model type</typeparam>
    /// <typeparam name="TDtoIn">dto input type, accepted in creation action body</typeparam>
    /// <typeparam name="TDtoOut">dto output type, generated after creation successfully</typeparam>
    public abstract class ControllerMapperCAsync<TModel, TDtoIn, TDtoOut> : ControllerMapperCAsync<IServiceCrudAsync<TModel>, TModel, TDtoIn, TDtoOut>
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
        protected ControllerMapperCAsync(IServiceCrudAsync<TModel> service, ILogger<ControllerMapperCAsync<TModel, TDtoIn, TDtoOut>> logger) : base(service, logger) { }

        /// <summary>
        /// Controller constructor with service data persistence and logging perform.<br/>
        /// The follow parameters can be set by dependency injection.<br/>
        /// Using no logger performing.
        /// </summary>
        /// <param name="service">service to data persistence</param>
        protected ControllerMapperCAsync(IServiceCrudAsync<TModel> service) : base(service) { }
    }

    /// <summary>
    /// Versioned Controller [C]reate Operation implementation for entity model using service.
    /// <para>
    /// This controller constains by default the following actions:<br/><br/>
    /// </para>
    /// <para>
    /// ┌[C]reate:<br/>
    /// └─► <see cref="Create(TDtoIn)"/>
    /// </para>
    /// </summary>
    /// <typeparam name="TService">target service to data persistence</typeparam>
    /// <typeparam name="TModel">entity model type</typeparam>
    /// <typeparam name="TDtoIn">dto input type, accepted in creation action body</typeparam>
    /// <typeparam name="TDtoOut">dto output type, generated after creation successfully</typeparam>
    public abstract class ControllerMapperCAsync<TService, TModel, TDtoIn, TDtoOut> : ControllerMapperCrudBaseAsync<TService, TModel>
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
        protected ControllerMapperCAsync(TService service, ILogger<ControllerMapperCAsync<TService, TModel, TDtoIn, TDtoOut>> logger) : base(service, logger) { }

        /// <summary>
        /// Controller constructor with service data persistence and logging perform.<br/>
        /// The follow parameters can be set by dependency injection.<br/>
        /// Using no logger performing.
        /// </summary>
        /// <param name="service">service to data persistence</param>
        protected ControllerMapperCAsync(TService service) : base(service) { }

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
    }
}
