using Com.Atomatus.Bootstarter.Model;
using Com.Atomatus.Bootstarter.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Com.Atomatus.Bootstarter.Web
{
    /// <summary>
    /// Versioned Controller [C]reate async operation implementation for entity model using service.
    /// <para>
    /// This controller constains by default the following actions:<br/><br/>
    /// </para>
    /// <para>
    /// ┌[C]reate:<br/>
    /// └─► <see cref="CreateAsync(TModel)"/>
    /// </para>
    /// </summary>
    /// <typeparam name="TService">target service to data persistence</typeparam>
    /// <typeparam name="TModel">entity model type</typeparam>
    /// <typeparam name="TID">entity model id type</typeparam>
    public abstract class ControllerCAsync<TService, TModel, TID> : ControllerCrudBaseAsync<TService, TModel, TID>
        where TService : IServiceCrudAsync<TModel, TID>
        where TModel : IModel<TID>
    {
        /// <summary>
        /// Controller constructor with service data persistence and logging perform.<br/>
        /// The follow parameters can be set by dependency injection.
        /// </summary>
        /// <param name="service">service to data persistence</param>
        /// <param name="logger">logging target</param>
        protected ControllerCAsync(TService service, ILogger<ControllerCAsync<TService, TModel, TID>> logger) : base(service, logger) { }

        /// <summary>
        /// Controller constructor with service data persistence and logging perform.<br/>
        /// The follow parameters can be set by dependency injection.<br/>
        /// Using no logger performing.
        /// </summary>
        /// <param name="service">service to data persistence</param>
        protected ControllerCAsync(TService service) : base(service) { }

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
        /// <returns>action result task</returns>        
        [HttpPost]
        public virtual Task<IActionResult> CreateAsync([FromBody] TModel result) => CreateActionAsync(result);
        #endregion

    }
}
