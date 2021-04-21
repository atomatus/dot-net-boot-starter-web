using Com.Atomatus.Bootstarter.Model;
using Com.Atomatus.Bootstarter.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Com.Atomatus.Bootstarter.Web
{
    /// <summary>
    /// Versioned Controller [C]reate and [U]pdate async operation implementation for entity model using service.
    /// <para>
    /// This controller constains by default the following actions:<br/><br/>
    /// </para>
    /// <para>
    /// ┌[C]reate:<br/>
    /// └─► <see cref="CreateAsync(TModel)"/>
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
    public abstract class ControllerCuAsync<TService, TModel, TID> : ControllerCrudBaseAsync<TService, TModel, TID>
        where TService : IServiceCrudAsync<TModel, TID>
        where TModel : IModel<TID>
    {
        /// <summary>
        /// Controller constructor with service data persistence and logging perform.<br/>
        /// The follow parameters can be set by dependency injection.
        /// </summary>
        /// <param name="service">service to data persistence</param>
        /// <param name="logger">logging target</param>
        protected ControllerCuAsync(TService service, ILogger<ControllerCuAsync<TService, TModel, TID>> logger) : base(service, logger) { }

        /// <summary>
        /// Controller constructor with service data persistence and logging perform.<br/>
        /// The follow parameters can be set by dependency injection.<br/>
        /// Using no logger performing.
        /// </summary>
        /// <param name="service">service to data persistence</param>
        protected ControllerCuAsync(TService service) : base(service) { }

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
