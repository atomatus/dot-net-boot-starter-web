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
    /// <para>
    /// API Controller CRUD base async implementing
    /// the default route "api/v{version:apiVersion}/[controller]" 
    /// and produces an "application/json" as default.
    /// </para>
    /// <para>
    /// This controller constains by default the following actions:<br/><br/>
    /// </para>
    /// 
    /// <para>
    /// ┌[C]reate:<br/>
    /// └─► <see cref="CreateActionAsync(TModel)"/>
    /// </para>
    /// 
    /// <para>
    /// ┌[R]ead:<br/>
    /// ├─► <see cref="GetActionAsync(CancellationToken)"/><br/>
    /// ├─► <see cref="GetActionAsync(Guid)"/><br/>
    /// </para>
    /// 
    /// <para>
    /// ┌[U]pdate:<br/>
    /// └─► <see cref="UpdateActionAsync(TModel)"/>
    /// </para>
    /// 
    /// <para>
    /// ┌[D]elete:<br/>
    /// └─► <see cref="DeleteActionAsync(Guid)"/>
    /// </para>
    /// </summary>
    /// <typeparam name="TService">target service type to data persistence</typeparam>
    /// <typeparam name="TModel">target model type</typeparam>  
    public abstract class ControllerCrudBaseAsync<TService, TModel> : ControllerBase<TService, TModel>
        where TService : IServiceCrudAsync<TModel>
        where TModel : IModel
    {
        /// <summary>
        /// Controller CRUD base constructor with service data persistence and logging perform.<br/>
        /// The follow parameters can be set by dependency injection.
        /// </summary>
        /// <param name="service">service to data persistence</param>
        /// <param name="logger">logging target</param>
        protected ControllerCrudBaseAsync(TService service, ILogger<ControllerCrudBaseAsync<TService, TModel>> logger) : base(service, logger) { }

        /// <summary>
        /// Controller CRUD base constructor with service data persistence and logging perform.<br/>
        /// The follow parameters can be set by dependency injection.<br/>
        /// Using no logger performing.
        /// </summary>
        /// <param name="service">service to data persistence</param>
        protected ControllerCrudBaseAsync(TService service) : base(service) { }

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
        [NonAction]
        protected async Task<IActionResult> CreateActionAsync(TModel result)
        {
            try
            {
                if (await service.ExistsAsync(result))
                {
                    throw new InvalidOperationException("Already exists a register with this UUID!");
                }

                return Ok(await service.SaveAsync(result));
            }
            catch (Exception ex)
            {
                logger.LogE(ex);
                return BadRequest(ex.Message);
            }
        }
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
        /// <returns>action result task</returns>    
        [NonAction]
        protected async Task<IActionResult> GetActionAsync(CancellationToken cancellationToken)
        {
            try
            {
                var result = await service.ListAsync(cancellationToken);

                if (result.Count == 0)
                {
                    logger.LogD("No content!");
                    return NoContent();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogE(ex);
                return BadRequest(ex.Message);
            }
        }

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
        [NonAction]
        protected async Task<IActionResult> GetActionAsync(Guid uuid)
        {
            try
            {
                if (uuid == default)
                {
                    throw new ArgumentException("Invalid uuid!");
                }

                var result = await service.GetByUuidAsync(uuid);
                if (result == null)
                {
                    logger.LogD("Uuid {0} not found to {1}.",
                        args: new object[] { uuid, typeof(TModel).Name });
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogE(ex);
                return BadRequest(ex.Message);
            }
        }

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
        [NonAction]
        protected async Task<IActionResult> PagingActionAsync(int page, int limit, CancellationToken cancellationToken)
        {
            try
            {
                var result = await service.PagingAsync(page, limit, cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogE(ex);
                return BadRequest(ex.Message);
            }
        }
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
        [NonAction]
        protected async Task<IActionResult> UpdateActionAsync(TModel result)
        {
            try
            {
                if (await service.ExistsAsync(result))
                {
                    await service.UpdateAsync(result);
                    return Ok();
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                logger.LogE(ex);
                return BadRequest(ex.Message);
            }
        }
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
        /// <returns>action result task</returns>        
        [NonAction]
        protected async Task<IActionResult> DeleteActionAsync(Guid uuid)
        {
            try
            {
                if (uuid == default)
                {
                    throw new ArgumentException("Invalid uuid!");
                }
                else if (!await service.ExistsByUuidAsync(uuid))
                {
                    logger.LogD("Uuid {0} not found to {1}.",
                        args: new object[] { uuid, typeof(TModel).Name });
                    return NotFound();
                }
                else if (await service.DeleteByUuidAsync(uuid))
                {
                    return Ok();
                }

                throw new InvalidOperationException("Was not possible to remove value!");
            }
            catch (Exception ex)
            {
                logger.LogE(ex);
                return BadRequest(ex.Message);
            }
        }
        #endregion
    }

    /// <summary>
    /// <para>
    /// API Controller CRUD base async implementing
    /// the default route "api/v{version:apiVersion}/[controller]" 
    /// and produces an "application/json" as default.
    /// </para>
    /// <para>
    /// This controller constains by default the following actions:<br/><br/>
    /// </para>
    /// 
    /// <para>
    /// ┌[C]reate:<br/>
    /// └─► <see cref="ControllerCrudBaseAsync{TService, TModel}.CreateActionAsync(TModel)"/>
    /// </para>
    /// 
    /// <para>
    /// ┌[R]ead:<br/>
    /// ├─► <see cref="ControllerCrudBaseAsync{TService, TModel}.GetActionAsync(CancellationToken)"/><br/>
    /// ├─► <see cref="ControllerCrudBaseAsync{TService, TModel}.GetActionAsync(Guid)"/><br/>
    /// └─► <see cref="GetActionAsync(ID)"/>
    /// </para>
    /// 
    /// <para>
    /// ┌[U]pdate:<br/>
    /// └─► <see cref="ControllerCrudBaseAsync{TService, TModel}.UpdateActionAsync(TModel)"/>
    /// </para>
    /// 
    /// <para>
    /// ┌[D]elete:<br/>
    /// └─► <see cref="ControllerCrudBaseAsync{TService, TModel}.DeleteActionAsync(Guid)"/>
    /// </para>
    /// </summary>
    /// <typeparam name="TService">target service type to data persistence</typeparam>
    /// <typeparam name="TModel">target model type</typeparam>  
    /// <typeparam name="ID">target id model type</typeparam>
    public abstract class ControllerCrudBaseAsync<TService, TModel, ID> : ControllerCrudBaseAsync<TService, TModel>
        where TService : IServiceCrudAsync<TModel, ID>
        where TModel : IModel<ID>
    {
        /// <summary>
        /// Controller CRUD base constructor with service data persistence and logging perform.<br/>
        /// The follow parameters can be set by dependency injection.
        /// </summary>
        /// <param name="service">service to data persistence</param>
        /// <param name="logger">logging target</param>
        protected ControllerCrudBaseAsync(TService service, ILogger<ControllerCrudBaseAsync<TService, TModel, ID>> logger) : base(service, logger) { }
        
        /// <summary>
        /// Controller CRUD base constructor with service data persistence and logging perform.<br/>
        /// The follow parameters can be set by dependency injection.<br/>
        /// Using no logger performing.
        /// </summary>
        /// <param name="service">service to data persistence</param>
        protected ControllerCrudBaseAsync(TService service) : base(service) { }

        #region [R]ead        
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
        [NonAction]
        protected async Task<IActionResult> GetActionAsync(ID id)
        {
            try
            {
                this.RequireValidId(id);

                var result = await service.GetAsync(id);

                if (result == null)
                {
                    logger.LogD("Id {0} NotFound", args: id);
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogE(ex);
                return BadRequest(ex.Message);
            }
        }
        #endregion
    }
}
