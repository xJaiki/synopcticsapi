using System;
using System.Collections.Generic;
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
    /// API Controller for plant model operations
    /// </summary>
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/v2")]
    public class PlantModelController : ApiController {
        private readonly IPlantModelTreeRepository _repository;

        public PlantModelController(IPlantModelTreeRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Gets the complete plant model tree with hierarchical structure
        /// </summary>
        /// <returns>The hierarchical plant model tree</returns>
        [HttpPost]
        [Route("GetPlantModelTree")]
        public async Task<IHttpActionResult> GetPlantModelTree()
        {
            try
            {
                var plantModel = await _repository.GetPlantModelTreeTreeAsync();

                // Create response in the expected format
                var response = new PlantModelTreeResponse
                {
                    EquipmentList = new List<EquipmentTreeDto>
                    {
                        new EquipmentTreeDto
                        {
                            Children = plantModel
                        }
                    },
                    ErrorList = new List<ErrorItemDto>()
                };

                return CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// CORS preflight request handler
        /// </summary>
        [HttpOptions]
        [Route("GetPlantModelTree")]
        public IHttpActionResult GetPlantModelTreeOptions()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            AddCorsHeaders(response);
            return ResponseMessage(response);
        }

        #region Helper Methods
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
                    ErrorList = new List<ErrorItemDto>
                    {
                        new ErrorItemDto(errorMessage, errorId)
                    }
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
            response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
            response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Accept");
            response.Headers.Add("Access-Control-Max-Age", "86400");
        }
        #endregion
    }
}