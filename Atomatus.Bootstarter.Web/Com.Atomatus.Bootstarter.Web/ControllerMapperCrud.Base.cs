
using System;
using System.Collections.Generic;
using Com.Atomatus.Bootstarter.Model;
using Com.Atomatus.Bootstarter.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Com.Atomatus.Bootstarter.Web
{
    /// <summary>
    /// <inheritdoc/>
    /// <para>
    /// API Controller CRUD for DTO Mapper base implementing
    /// the default route "api/v{version:apiVersion}/[controller]" 
    /// and produces an "application/json" as default.
    /// </para>
    /// <para>
    /// This controller constains by default the following actions:<br/><br/>
    /// </para>
    /// 
    /// <para>
    /// ┌[C]reate:<br/>
    /// ├─► <see cref="CreateAction{TDtoIn, TDtoOut}(TDtoIn)"/>
    /// └─► <see cref="CreateAction{TDtoIn}(TDtoIn)"/>
    /// </para>
    /// 
    /// <para>
    /// ┌[R]ead:<br/>
    /// ├─► <see cref="GetAction{TDtoOut}()"/><br/>
    /// ├─► <see cref="GetAction{TDtoOut}(Guid)"/><br/>
    /// └─► <see cref="PagingAction{TDtoOut}(int, int)"/><br/>
    /// </para>
    /// 
    /// <para>
    /// ┌[U]pdate:<br/>
    /// └─► <see cref="UpdateAction{TDtoIn}(TDtoIn)"/>
    /// </para>
    /// 
    /// <para>
    /// ┌[D]elete:<br/>
    /// └─► <see cref="DeleteAction(Guid)"/>
    /// </para>
    /// </summary>
    /// <typeparam name="TService">target service type to data persistence</typeparam>
    /// <typeparam name="TModel">target model type</typeparam>
    public abstract class ControllerMapperCrudBase<TService, TModel> : ControllerMapperBase<TService, TModel>
        where TService : IServiceCrud<TModel>
        where TModel : class, IModel, new()
    {
        /// <inheritdoc/>
        protected ControllerMapperCrudBase(TService service) : base(service) { }

        /// <inheritdoc/>
        protected ControllerMapperCrudBase(TService service, ILogger<ControllerMapperCrudBase<TService, TModel>> logger) : base(service, logger) { }

        #region [C]reate
        /// <summary>
        /// <para>Perform a write operation to persist data.</para>
        /// <i>https://api.urladdress/v1 (POST Method/ DTO data from body)</i>
        /// <para>
        /// Results<br/>
        /// ● OK: Successfully, contains DTO with id.<br/>
        /// ● Bad Request: Aleady exists or some another error.
        /// </para>
        /// </summary>
        /// <param name="dtoResult">DTO from body</param>
        /// <returns>action result</returns>
        /// <typeparam name="TDtoIn">DTO type source from body</typeparam>
        /// <typeparam name="TDtoOut">DTO type result</typeparam>
        [NonAction]
        protected IActionResult CreateAction<TDtoIn, TDtoOut>(TDtoIn dtoResult)
            where TDtoIn : class, new()
            where TDtoOut : class, new()
        {
            try
            {
                if(dtoResult is null)
                {
                    throw new ArgumentNullException(nameof(dtoResult));
                }

                TModel result = this.Parse<TDtoIn, TModel>(dtoResult);

                if (service.Exists(result))
                {
                    throw new InvalidOperationException("This register already exists!");
                }

                result = service.Save(result);
                TDtoOut dtoOut = this.Parse<TModel, TDtoOut>(result);
                return Ok(dtoOut);
            }
            catch (Exception ex)
            {
                logger.LogE(ex);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// <para>Perform a write operation to persist data.</para>
        /// <i>https://api.urladdress/v1 (POST Method/ DTO data from body)</i>
        /// <para>
        /// Results<br/>
        /// ● OK: Successfully, contains DTO with id.<br/>
        /// ● Bad Request: Aleady exists or some another error.
        /// </para>
        /// </summary>
        /// <param name="dtoResult">DTO from body</param>
        /// <returns>action result</returns>
        /// <typeparam name="TDtoIn">DTO type source and result</typeparam>
        [NonAction]
        protected IActionResult CreateAction<TDtoIn>(TDtoIn dtoResult)
            where TDtoIn : class, new()
        {
            return CreateAction<TDtoIn, TDtoIn>(dtoResult);
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
        /// ● OK: Successfully, contains dto result list.<br/>
        /// ● Bad Request: some error in request.
        /// </para>
        /// </summary>
        /// <returns>action result</returns>
        /// <typeparam name="TDtoOut">dto result type</typeparam>
        [NonAction]
        protected IActionResult GetAction<TDtoOut>()
            where TDtoOut : class, new()
        {
            try
            {
                var result = service.List();

                if (result.Count == 0)
                {
                    logger.LogD("No content!");
                    return NoContent();
                }

                var dtoResult = this.ParseList<TModel, TDtoOut>(result);
                return Ok(dtoResult);
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
        /// ● OK: Successfully, contains dto result.<br/>
        /// ● Not Found: Does not exists register with uuid.<br/>
        /// ● Bad Request: some error in request.
        /// </para>
        /// </summary>
        /// <param name="uuid">targer uuid</param>
        /// <returns>action result</returns>    
        /// <typeparam name="TDtoOut">dto result type</typeparam>
        [NonAction]
        protected IActionResult GetAction<TDtoOut>(Guid uuid)
            where TDtoOut : class, new()
        {
            try
            {
                if (uuid == default)
                {
                    throw new ArgumentException("Invalid uuid!");
                }

                var result = service.GetByUuid(uuid);
                if (result is null)
                {
                    logger.LogD("Uuid {0} not found to {1}.",
                        args: new object[] { uuid, typeof(TModel).Name });
                    return NotFound();
                }

                var dtoResult = this.Parse<TModel, TDtoOut>(result);
                return Ok(dtoResult);
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
        /// ● OK: Successfully, contains dto result or empty result.<br/>
        /// ● Bad Request: some error in request.
        /// </para>
        /// </summary>
        /// <param name="page">page index, from 0</param>
        /// <param name="limit">page limit request</param>
        /// <returns>action result</returns> 
        /// <typeparam name="TDtoOut">dto result type</typeparam>   
        [NonAction]
        protected IActionResult PagingAction<TDtoOut>(int page, int limit)
            where TDtoOut : class, new()
        {
            try
            {
                var result = service.Paging(page, limit);
                var dtoResult = this.ParseList<TModel, TDtoOut>(result);
                return Ok(dtoResult);
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
        /// <i>https://api.urladdress/v1 (PUT Method/ DTO data from body)</i>
        /// <para>
        /// Results<br/>
        /// ● OK: Successfully, data updated.<br/>
        /// ● Not Found: target data does not exists.<br/>
        /// ● Bad Request: some error.
        /// </para>
        /// </summary>
        /// <typeparam name="TDtoIn">DTO type source from body</typeparam>
        /// <param name="dtoResult">dto from body</param>
        /// <returns>action result</returns>        
        [NonAction]
        protected IActionResult UpdateAction<TDtoIn>(TDtoIn dtoResult) where TDtoIn : class, new()
        {
            try
            {
                if (dtoResult is null)
                {
                    throw new ArgumentNullException(nameof(dtoResult));
                }

                TModel result = this.Parse<TDtoIn, TModel>(dtoResult);

                if (service.Exists(result))
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
}

