using Com.Atomatus.Bootstarter.Model;
using Com.Atomatus.Bootstarter.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace Com.Atomatus.Bootstarter.Web
{
    /// <summary>
    /// <para>
    /// API Controller CRUD base implementing
    /// the default route "api/v{version:apiVersion}/[controller]" 
    /// and produces an "application/json" as default.
    /// </para>
    /// <para>
    /// This controller constains by default the following actions:<br/><br/>
    /// </para>
    /// 
    /// <para>
    /// ┌[C]reate:<br/>
    /// └─► <see cref="CreateAction(TModel)"/>
    /// </para>
    /// 
    /// <para>
    /// ┌[R]ead:<br/>
    /// ├─► <see cref="GetAction()"/><br/>
    /// ├─► <see cref="GetAction(Guid)"/><br/>
    /// └─► <see cref="PagingAction(int, int)"/><br/>
    /// </para>
    /// 
    /// <para>
    /// ┌[U]pdate:<br/>
    /// └─► <see cref="UpdateAction(TModel)"/>
    /// </para>
    /// 
    /// <para>
    /// ┌[D]elete:<br/>
    /// └─► <see cref="DeleteAction(Guid)"/>
    /// </para>
    /// </summary>
    /// <typeparam name="TService">target service type to data persistence</typeparam>
    /// <typeparam name="TModel">target model type</typeparam>
    public abstract class ControllerCrudBase<TService, TModel> : ControllerBase<TService, TModel>
        where TService : IServiceCrud<TModel>
        where TModel : IModel
    {
        /// <summary>
        /// Controller CRUD base constructor with service data persistence and logging perform.<br/>
        /// The follow parameters can be set by dependency injection.
        /// </summary>
        /// <param name="service">service to data persistence</param>
        /// <param name="logger">logging target</param>
        protected ControllerCrudBase(TService service, ILogger<ControllerCrudBase<TService, TModel>> logger) : base(service, logger) { }

        /// <summary>
        /// Controller CRUD base constructor with service data persistence and logging perform.<br/>
        /// The follow parameters can be set by dependency injection.<br/>
        /// Using no logger performing.
        /// </summary>
        /// <param name="service">service to data persistence</param>
        protected ControllerCrudBase(TService service) : base(service) { }

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
        [NonAction]
        protected IActionResult CreateAction(TModel result)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new ArgumentException("Invalid data format!");
                }
                else if (service.Exists(result))
                {
                    throw new InvalidOperationException("Already exists a register with this UUID!");
                }

                return Ok(service.Save(result));
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
        /// </summary>
        /// <returns>action result</returns>    
        [NonAction]
        protected IActionResult GetAction()
        {
            try
            {
                var result = service.List();

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
        /// <returns>action result</returns>    
        [NonAction]
        protected IActionResult GetAction(Guid uuid)
        {
            try
            {
                if (uuid == default)
                {
                    throw new ArgumentException("Invalid uuid!");
                }
                else if (!ModelState.IsValid)
                {
                    throw new ArgumentException("Invalid data format!");
                }

                var result = service.GetByUuid(uuid);
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
        /// <param name="page">page index, from 0</param>
        /// <param name="limit">page limit request</param>
        /// <returns>action result</returns>    
        [NonAction]
        protected IActionResult PagingAction(int page, int limit)
        {
            try
            {
                var result = service.Paging(page, limit);
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
        /// <returns>action result</returns>        
        [NonAction]
        protected IActionResult UpdateAction(TModel result)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new ArgumentException("Invalid data format!");
                }
                else if (service.Exists(result))
                {
                    service.Update(result);
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
        /// <returns>action result</returns>        
        [NonAction]
        public virtual IActionResult DeleteAction(Guid uuid)
        {
            try
            {
                if (uuid == default)
                {
                    throw new ArgumentException("Invalid uuid!");
                }
                else if (!ModelState.IsValid)
                {
                    throw new ArgumentException("Invalid data format!");
                }
                else if (!service.ExistsByUuid(uuid))
                {
                    logger.LogD("Uuid {0} not found to {1}.",
                        args: new object[] { uuid, typeof(TModel).Name });
                    return NotFound();
                }
                else if (service.DeleteByUuid(uuid))
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
    /// API Controller CRUD base implementing
    /// the default route "api/v{version:apiVersion}/[controller]" 
    /// and produces an "application/json" as default.
    /// </para>
    /// <para>
    /// This controller constains by default the following actions:<br/><br/>
    /// </para>
    /// 
    /// <para>
    /// ┌[C]reate:<br/>
    /// └─► <see cref="ControllerCrudBase{TService, TModel}.CreateAction(TModel)"/>
    /// </para>
    /// 
    /// <para>
    /// ┌[R]ead:<br/>
    /// ├─► <see cref="ControllerCrudBase{TService, TModel}.GetAction()"/><br/>
    /// ├─► <see cref="ControllerCrudBase{TService, TModel}.GetAction(Guid)"/><br/>
    /// └─► <see cref="GetAction(TID)"/>
    /// </para>
    /// 
    /// <para>
    /// ┌[U]pdate:<br/>
    /// └─► <see cref="ControllerCrudBase{TService, TModel}.UpdateAction(TModel)"/>
    /// </para>
    /// 
    /// <para>
    /// ┌[D]elete:<br/>
    /// └─► <see cref="ControllerCrudBase{TService, TModel}.DeleteAction(Guid)"/>
    /// </para>
    /// </summary>
    /// <typeparam name="TService">target service type to data persistence</typeparam>
    /// <typeparam name="TModel">target model type</typeparam>  
    /// <typeparam name="TID">target id model type</typeparam>
    public abstract class ControllerCrudBase<TService, TModel, TID> : ControllerCrudBase<TService, TModel>
        where TService : IServiceCrud<TModel, TID>
        where TModel : IModel<TID>
    {
        /// <summary>
        /// Controller CRUD base constructor with service data persistence and logging perform.<br/>
        /// The follow parameters can be set by dependency injection.
        /// </summary>
        /// <param name="service">service to data persistence</param>
        /// <param name="logger">logging target</param>
        protected ControllerCrudBase(TService service, ILogger<ControllerCrudBase<TService, TModel, TID>> logger) : base(service, logger) { }

        /// <summary>
        /// Controller CRUD base constructor with service data persistence and logging perform.<br/>
        /// The follow parameters can be set by dependency injection.<br/>
        /// Using no logger performing.
        /// </summary>
        /// <param name="service">service to data persistence</param>
        protected ControllerCrudBase(TService service) : base(service) { }

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
        /// <returns>action result</returns>    
        [NonAction]
        protected IActionResult GetAction(TID id)
        {
            try
            {
                this.RequireValidId(id);

                var result = service.Get(id);

                if (result == null)
                {
                    logger.LogD("Id {0} not found!", args: id);
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
