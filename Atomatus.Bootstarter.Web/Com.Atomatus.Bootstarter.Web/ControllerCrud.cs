using Com.Atomatus.Bootstarter.Model;
using Com.Atomatus.Bootstarter.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace Com.Atomatus.Bootstarter.Web
{
    public abstract class ControllerCrud<Service, Model, ID> : ControllerBase<Service, Model>
        where Service : IServiceCrud<Model, ID>
        where Model : IModel<ID>
    {
        protected ControllerCrud(Service service, ILogger<ControllerCrud<Service, Model, ID>> logger) : base(service, logger) { }

        protected ControllerCrud(Service service) : base(service) { }

        #region [C]reate
        [HttpPost]
        public IActionResult Create([FromBody] Model result)
        {
            try
            {
                if(service.Exists(result))
                {
                    throw new InvalidOperationException("Already exists a register with this UUID!");
                }

                return Ok(service.Insert(result));
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
        public virtual IActionResult Get()
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

        [HttpGet("{id}")]
        public virtual IActionResult Get(ID id)
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

        [HttpGet("uuid/{uuid}")]
        public virtual IActionResult Get(Guid uuid)
        {
            try
            {
                if(uuid == default)
                {
                    throw new ArgumentException("Invalid uuid!");
                }

                var result = service.GetByUuid(uuid);
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
        public virtual IActionResult Paging(int page, int limit = -1)
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
        [HttpPut]
        public IActionResult Update([FromBody] Model result)
        {
            try
            {
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
        [HttpDelete("{uuid}")]
        public IActionResult Delete(Guid uuid)
        {
            try
            {
                if(uuid == default)
                {
                    throw new ArgumentException("Invalid uuid!");
                }
                else if (!service.Exists(uuid))
                {
                    logger.LogD("Uuid {0} not found to {1}.",
                        args: new object[] { uuid, typeof(Model).Name });
                    return NotFound();
                }
                else if (service.Delete(uuid))
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
