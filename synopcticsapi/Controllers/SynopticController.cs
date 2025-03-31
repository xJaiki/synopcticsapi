using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Newtonsoft.Json;
using synopcticsapi.Models;
using synopcticsapi.Repository;

namespace synopcticsapi.Controllers {
    /// <summary>
    /// API Controller for synoptic operations
    /// </summary>
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/v2")]
    public class SynopticController : ApiController {
        private readonly ISynopticRepository _repository;

        public SynopticController(ISynopticRepository repository)
        {
            _repository = repository;
        }

        #region DTO Classes
        // DTOs per le richieste
        public class SynopticRequest {
            public string SynopticName { get; set; }
        }

        public class SynopticModifyRequest {
            public int AreaId { get; set; }
            public string SynopticName { get; set; }
            public string SynopticSVG { get; set; }
        }

        // DTO per le risposte
        private class ApiResponse<T> {
            public T Data { get; set; }
            public List<ErrorItem> ErrorList { get; set; } = new List<ErrorItem>();
        }

        private class ErrorItem {
            public string Description { get; set; }
            public int Id { get; set; }

            public ErrorItem(string description, int id = 0)
            {
                Description = description;
                Id = id;
            }
        }

        private class SynopticListResponse {
            public List<SynopticDto> SynopticList { get; set; }
            public List<ErrorItem> ErrorList { get; set; } = new List<ErrorItem>();
        }

        private class SynopticDto {
            public string Name { get; set; }
            public string AreaId { get; set; }
            public string Svg { get; set; }
        }
        #endregion

        #region GET Methods
        /// <summary>
        /// Gets a list of all synoptic layouts
        /// </summary>
        /// <returns>A list of synoptic layouts</returns>
        [HttpPost]
        [Route("GetSynopticList")]
        public async Task<IHttpActionResult> GetSynopticList()
        {
            try
            {
                var synoptics = await _repository.GetSynopticListAsync();
                var mappedSynoptics = synoptics.Select(s => new SynopticDto
                {
                    Name = s.Layout,
                    AreaId = s.AreaId,
                    Svg = s.Svg
                }).ToList();

                return CreateResponse(HttpStatusCode.OK, new SynopticListResponse
                {
                    SynopticList = mappedSynoptics
                });
            }
            catch (Exception ex)
            {
                return CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("GetSynoptic")]
        public async Task<IHttpActionResult> GetSynoptic([FromBody] SynopticRequest request)
        {
            try
            {
                if (request == null || string.IsNullOrEmpty(request.SynopticName))
                {
                    return CreateErrorResponse(HttpStatusCode.BadRequest, "Synoptic name is required");
                }

                var synoptic = await _repository.GetSynopticAsync(request.SynopticName);

                if (synoptic == null)
                {
                    return CreateErrorResponse(HttpStatusCode.NotFound, "Synoptic not found");
                }

                return CreateResponse(HttpStatusCode.OK, new SynopticListResponse
                {
                    SynopticList = new List<SynopticDto>
                    {
                        new SynopticDto
                        {
                            Name = synoptic.Layout,
                            Svg = synoptic.Svg,
                            AreaId = synoptic.AreaId
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                return CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        #endregion

        #region Modify Methods
        /// <summary>
        /// Inserts a new synoptic layout
        /// </summary>
        /// <param name="request">The synoptic layout to insert</param>
        /// <returns>The result of the operation</returns>
        [HttpPost]
        [Route("InsertSynoptic")]
        public async Task<IHttpActionResult> InsertSynoptic([FromBody] SynopticModifyRequest request)
        {
            return await ModifySynoptic(request, async (synopticLayout) =>
                await _repository.InsertSynopticAsync(synopticLayout),
                HttpStatusCode.Created,
                "Synoptic saved successfully",
                "A synoptic with this name already exists",
                HttpStatusCode.Conflict);
        }

        /// <summary>
        /// Updates an existing synoptic layout
        /// </summary>
        /// <param name="request">The synoptic layout to update</param>
        /// <returns>The result of the operation</returns>
        [HttpPost]
        [Route("UpdateSynoptic")]
        public async Task<IHttpActionResult> UpdateSynoptic([FromBody] SynopticModifyRequest request)
        {
            return await ModifySynoptic(request, async (synopticLayout) =>
                await _repository.UpdateSynopticAsync(synopticLayout),
                HttpStatusCode.OK,
                "Synoptic updated successfully",
                "Synoptic not found",
                HttpStatusCode.NotFound);
        }

        /// <summary>
        /// Handles both insert and update operations
        /// </summary>
        private async Task<IHttpActionResult> ModifySynoptic(
            SynopticModifyRequest request,
            Func<SynopticLayout, Task<bool>> repositoryAction,
            HttpStatusCode successCode,
            string successMessage,
            string failureMessage,
            HttpStatusCode failureCode)
        {
            if (request == null)
            {
                return CreateErrorResponse(HttpStatusCode.BadRequest, "Synoptic data is required", 0);
            }

            if (string.IsNullOrEmpty(request.SynopticName) || string.IsNullOrEmpty(request.SynopticSVG))
            {
                return CreateErrorResponse(HttpStatusCode.BadRequest, "Synoptic name and SVG content are required", 0);
            }

            try
            {
                var synopticLayout = new SynopticLayout
                {
                    Layout = request.SynopticName,
                    AreaId = request.AreaId.ToString(),
                    Svg = request.SynopticSVG
                };

                bool result = await repositoryAction(synopticLayout);

                if (result)
                {
                    return CreateErrorResponse(successCode, successMessage, 1);
                }
                else
                {
                    return CreateErrorResponse(failureCode, failureMessage, 0);
                }
            }
            catch (Exception ex)
            {
                return CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message, 0);
            }
        }
        #endregion

        #region OPTIONS Methods
        [HttpOptions]
        [Route("GetSynopticList")]
        public IHttpActionResult GetSynopticListOptions()
        {
            return CreateOptionsResponse();
        }

        [HttpOptions]
        [Route("GetSynoptic")]
        public IHttpActionResult GetSynopticOptions()
        {
            return CreateOptionsResponse();
        }

        [HttpOptions]
        [Route("InsertSynoptic")]
        public IHttpActionResult InsertSynopticOptions()
        {
            return CreateOptionsResponse();
        }

        [HttpOptions]
        [Route("UpdateSynoptic")]
        public IHttpActionResult UpdateSynopticOptions()
        {
            return CreateOptionsResponse();
        }
        #endregion

        #region Helper Methods
        private IHttpActionResult CreateOptionsResponse()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            AddCorsHeaders(response);
            return ResponseMessage(response);
        }

        private IHttpActionResult CreateResponse<T>(HttpStatusCode statusCode, T data)
        {
            var response = new HttpResponseMessage(statusCode);
            response.Content = new StringContent(
                JsonConvert.SerializeObject(data),
                Encoding.UTF8,
                "application/json"
            );

            AddCorsHeaders(response);
            return ResponseMessage(response);
        }

        private IHttpActionResult CreateErrorResponse(HttpStatusCode statusCode, string errorMessage, int errorId = 0)
        {
            var response = new HttpResponseMessage(statusCode);
            response.Content = new StringContent(
                JsonConvert.SerializeObject(new
                {
                    ErrorList = new List<ErrorItem> { new ErrorItem(errorMessage, errorId) }
                }),
                Encoding.UTF8,
                "application/json"
            );

            AddCorsHeaders(response);
            return ResponseMessage(response);
        }

        private void AddCorsHeaders(HttpResponseMessage response)
        {
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, OPTIONS");
            response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Accept");
            response.Headers.Add("Access-Control-Max-Age", "86400");
        }
        #endregion
    }
}