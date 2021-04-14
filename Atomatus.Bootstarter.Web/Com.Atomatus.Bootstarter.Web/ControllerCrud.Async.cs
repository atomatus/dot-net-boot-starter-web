using Com.Atomatus.Bootstarter.Model;
using Com.Atomatus.Bootstarter.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Com.Atomatus.Bootstarter.Web
{
    public abstract class ControllerCrudAsync<Service, Model, ID> : ControllerBase<Service, Model>
        where Service : IServiceCrudAsync<Model, ID>
        where Model : IModel<ID>
    {
        protected ControllerCrudAsync(Service service, ILogger<ControllerCrudAsync<Service, Model, ID>> logger) : base(service, logger) { }

        protected ControllerCrudAsync(Service service) : base(service) { }

        #region [C]reate
        [HttpPost]
        public virtual async Task<IActionResult> CreateAsync([FromBody] Model result)
        {
            try
            {
                if (await service.ExistsAsync(result))
                {
                    throw new InvalidOperationException("Already exists a register with this UUID!");
                }

                return Ok(await service.InsertAsync(result));
            }
            catch (Exception ex)
            {
                logger.LogE(ex);
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region [R]ead
        [HttpGet]
        public virtual async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
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
        
        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetAsync(ID id)
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

        [HttpGet("uuid/{uuid}")]
        public virtual async Task<IActionResult> GetAsync(Guid uuid)
        {
            try
            {
                if (uuid == default)
                {
                    throw new ArgumentException("Invalid uuid!");
                }

                var result = await service.GetAsync(uuid);
                if (result == null)
                {
                    logger.LogD("Uuid {0} not found to {1}.",
                        args: new object[] { uuid, typeof(Model).Name });
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

        [HttpGet("page/{page}/{limit:int?}")]
        public virtual async Task<IActionResult> PagingAsync(int page, int limit = -1, CancellationToken cancellationToken = default)
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
        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] Model result)
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
        [HttpDelete("{uuid}")]
        public async Task<IActionResult> DeleteAsync(Guid uuid)
        {
            try
            {
                if (uuid == default)
                {
                    throw new ArgumentException("Invalid uuid!");
                }
                else if (!await service.ExistsAsync(uuid))
                {
                    logger.LogD("Uuid {0} not found to {1}.",
                        args: new object[] { uuid, typeof(Model).Name });
                    return NotFound();
                }
                else if (await service.DeleteAsync(uuid))
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
