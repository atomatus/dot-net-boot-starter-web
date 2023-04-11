using System;
using System.Threading;
using System.Threading.Tasks;
using Com.Atomatus.Bootstarter.Model;
using Com.Atomatus.Bootstarter.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Com.Atomatus.Bootstarter.Web
{
    /// <summary>
    /// <inheritdoc/>
    /// <para>
    /// API Controller CRUD for DTO Mapper base async implementing
    /// the default route "api/v{version:apiVersion}/[controller]" 
    /// and produces an "application/json" as default.
    /// </para>
    /// <para>
    /// This controller constains by default the following actions:<br/><br/>
    /// </para>
    /// 
    /// <para>
    /// ┌[C]reate:<br/>
    /// ├─► <see cref="CreateActionAsync{TDtoIn, TDtoOut}(TDtoIn)"/>
    /// └─► <see cref="CreateActionAsync{TDtoIn}(TDtoIn)"/>
    /// </para>
    /// 
    /// <para>
    /// ┌[R]ead:<br/>
    /// ├─► <see cref="GetActionAsync{TDtoOut}(CancellationToken)"/><br/>
    /// ├─► <see cref="GetActionAsync{TDtoOut}(Guid)"/><br/>
    /// └─► <see cref="PagingActionAsync{TDtoOut}(int, int, CancellationToken)"/><br/>
    /// </para>
    /// 
    /// <para>
    /// ┌[U]pdate:<br/>
    /// └─► <see cref="UpdateActionAsync{TDtoIn}(TDtoIn)"/>
    /// </para>
    /// 
    /// <para>
    /// ┌[D]elete:<br/>
    /// └─► <see cref="DeleteActionAsync(Guid)"/>
    /// </para>
    /// </summary>
    /// <typeparam name="TService">target service type to data persistence</typeparam>
    /// <typeparam name="TModel">target model type</typeparam>
    public abstract class ControllerMapperCrudBaseAsync<TService, TModel> : ControllerMapperBase<TService, TModel>
        where TService : IServiceCrudAsync<TModel>
        where TModel : class, IModel, new()
    {
        /// <inheritdoc/>
        public ControllerMapperCrudBaseAsync(TService service) : base(service) { }

        /// <inheritdoc/>
        public ControllerMapperCrudBaseAsync(TService service, ILogger<ControllerMapperBase<TService, TModel>> logger) : base(service, logger) { }

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
        /// <param name="result">DTO from body</param>
        /// <returns>action result task</returns>    
        /// <typeparam name="TDtoIn">DTO type source from body</typeparam>
        /// <typeparam name="TDtoOut">DTO type result</typeparam>    
        [NonAction]
        protected async Task<IActionResult> CreateActionAsync<TDtoIn, TDtoOut>(TDtoIn dtoResult)
            where TDtoIn : class, new()
            where TDtoOut : class, new()
        {
            try
            {
                if (dtoResult is null)
                {
                    throw new ArgumentNullException(nameof(dtoResult));
                }

                TModel result = this.Parse<TDtoIn, TModel>(dtoResult);

                if (await service.ExistsAsync(result))
                {
                    throw new InvalidOperationException("This register already exists!");
                }

                result = await service.SaveAsync(result);
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
        protected Task<IActionResult> CreateActionAsync<TDtoIn>(TDtoIn dtoResult)
            where TDtoIn : class, new()
        {
            return CreateActionAsync<TDtoIn, TDtoIn>(dtoResult);
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
        /// <i> This operation can be cancelled.</i>
        /// </summary>
        /// <param name="cancellationToken">cancellation token</param>
        /// <returns>action result task</returns>    
        /// <typeparam name="TDtoOut">dto result type</typeparam>
        [NonAction]
        protected async Task<IActionResult> GetActionAsync<TDtoOut>(CancellationToken cancellationToken)
            where TDtoOut : class, new()
        {
            try
            {
                var result = await service.ListAsync(cancellationToken);

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
        /// <returns>action result task</returns>    
        /// <typeparam name="TDtoOut">dto result type</typeparam>
        [NonAction]
        protected async Task<IActionResult> GetActionAsync<TDtoOut>(Guid uuid)
            where TDtoOut : class, new()
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
        /// <i> This operation can be cancelled.</i>
        /// </summary>
        /// <param name="page">page index, from 0</param>
        /// <param name="limit">page limit request</param>
        /// <param name="cancellationToken">cancellation token</param>
        /// <returns>action result task</returns>    
        [NonAction]
        protected async Task<IActionResult> PagingActionAsync<TDtoOut>(int page, int limit, CancellationToken cancellationToken)
            where TDtoOut : class, new()
        {
            try
            {
                var result = await service.PagingAsync(page, limit, cancellationToken);
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
        /// <i>https://api.urladdress/v1 (PUT Method/ DTOdata from body)</i>
        /// <para>
        /// Results<br/>
        /// ● OK: Successfully, data dto updated.<br/>
        /// ● Not Found: target data dto does not exists.<br/>
        /// ● Bad Request: some error.
        /// </para>
        /// </summary>
        /// <param name="result">DTO from body</param>
        /// <returns>action result task</returns>        
        [NonAction]
        protected async Task<IActionResult> UpdateActionAsync<TDtoIn>(TDtoIn dtoResult) where TDtoIn : class, new()
        {
            try
            {
                if (dtoResult is null)
                {
                    throw new ArgumentNullException(nameof(dtoResult));
                }

                TModel result = this.Parse<TDtoIn, TModel>(dtoResult);

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
}

