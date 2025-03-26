using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using synopcticsapi.Models;
using synopcticsapi.Repository;

namespace synopcticsapi.Controllers {
    /// <summary>
    /// API Controller for synoptic operations
    /// </summary>
    [RoutePrefix("api/v2")]
    public class SynopticController : ApiController {
        private readonly ISynopticRepository _repository;

        public SynopticController(ISynopticRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Gets a list of all synoptic layouts
        /// </summary>
        /// <returns>A list of synoptic layouts</returns>
        [HttpGet]
        [Route("GetSynopticList")]
        public async Task<IHttpActionResult> GetSynopticList()
        {
            try
            {
                var synoptics = await _repository.GetSynopticListAsync();
                return Ok(synoptics);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Gets a specific synoptic layout by its identifier
        /// </summary>
        /// <param name="layout">The layout identifier</param>
        /// <returns>The synoptic layout</returns>
        [HttpGet]
        [Route("GetSynoptic")]
        public async Task<IHttpActionResult> GetSynoptic(string layout)
        {
            if (string.IsNullOrEmpty(layout))
            {
                return BadRequest("Layout identifier is required");
            }

            try
            {
                var synoptic = await _repository.GetSynopticAsync(layout);
                if (synoptic == null)
                {
                    return NotFound();
                }

                return Ok(synoptic);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Inserts a new synoptic layout
        /// </summary>
        /// <param name="synopticLayout">The synoptic layout to insert</param>
        /// <returns>The result of the operation</returns>
        [HttpPost]
        [Route("InsertSynoptic")]
        public async Task<IHttpActionResult> InsertSynoptic([FromBody] SynopticLayout synopticLayout)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (synopticLayout == null)
            {
                return BadRequest("Synoptic layout data is required");
            }

            try
            {
                bool result = await _repository.InsertSynopticAsync(synopticLayout);
                if (result)
                {
                    // Return 201 Created with the location of the new resource
                    return Created($"api/v2/GetSynoptic?layout={synopticLayout.Layout}", synopticLayout);
                }
                else
                {
                    // Return 409 Conflict if the synoptic already exists
                    return Content(HttpStatusCode.Conflict, "A synoptic with this layout already exists");
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Updates an existing synoptic layout
        /// </summary>
        /// <param name="synopticLayout">The synoptic layout to update</param>
        /// <returns>The result of the operation</returns>
        [HttpPut]
        [Route("UpdateSynoptic")]
        public async Task<IHttpActionResult> UpdateSynoptic([FromBody] SynopticLayout synopticLayout)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (synopticLayout == null)
            {
                return BadRequest("Synoptic layout data is required");
            }

            try
            {
                bool result = await _repository.UpdateSynopticAsync(synopticLayout);
                if (result)
                {
                    return Ok(synopticLayout);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}