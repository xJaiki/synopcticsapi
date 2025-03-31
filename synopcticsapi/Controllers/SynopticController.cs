using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
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
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent(
                    JsonConvert.SerializeObject(new
                    {
                        SynopticList = synoptics,
                        ErrorList = new List<object>()
                    }),
                    Encoding.UTF8,
                    "application/json"
                );

                // Aggiungi tutti i possibili header
                response.Headers.Add("Access-Control-Allow-Origin", "*");
                response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
                response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Accept");
                response.Headers.Add("Access-Control-Max-Age", "86400");

                return ResponseMessage(response);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpOptions]
        [Route("GetSynopticList")]
        public IHttpActionResult Options()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
            response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Accept");
            return ResponseMessage(response);
        }

        [HttpPost]
        [Route("GetSynoptic")]
        public async Task<IHttpActionResult> GetSynoptic([FromBody] SynopticRequest request)
        {
            try
            {
                if (request == null || string.IsNullOrEmpty(request.SynopticName))
                {
                    var errorResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    errorResponse.Content = new StringContent(
                        JsonConvert.SerializeObject(new
                        {
                            ErrorList = new List<object> { new { Description = "Synoptic name is required" } }
                        }),
                        Encoding.UTF8,
                        "application/json"
                    );

                    AddCorsHeaders(errorResponse);
                    return ResponseMessage(errorResponse);
                }

                var synoptic = await _repository.GetSynopticAsync(request.SynopticName);
                var response = new HttpResponseMessage(synoptic == null ? HttpStatusCode.NotFound : HttpStatusCode.OK);

                if (synoptic == null)
                {
                    response.Content = new StringContent(
                        JsonConvert.SerializeObject(new
                        {
                            ErrorList = new List<object> { new { Description = "Synoptic not found" } }
                        }),
                        Encoding.UTF8,
                        "application/json"
                    );
                }
                else
                {
                    // Questo formato deve corrispondere a ciò che il client si aspetta
                    response.Content = new StringContent(
                        JsonConvert.SerializeObject(new
                        {
                            SynopticList = new List<object>
                            {
                        new
                        {
                            Name = synoptic.Layout,
                            Svg = synoptic.Svg,
                            AreaId = synoptic.AreaId
                        }
                            },
                            ErrorList = new List<object>()
                        }),
                        Encoding.UTF8,
                        "application/json"
                    );
                }

                AddCorsHeaders(response);
                return ResponseMessage(response);
            }
            catch (Exception ex)
            {
                var errorResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                errorResponse.Content = new StringContent(
                    JsonConvert.SerializeObject(new
                    {
                        ErrorList = new List<object> { new { Description = ex.Message } }
                    }),
                    Encoding.UTF8,
                    "application/json"
                );

                AddCorsHeaders(errorResponse);
                return ResponseMessage(errorResponse);
            }
        }

        // Classe per ricevere la richiesta
        public class SynopticRequest {
            public string SynopticName { get; set; }
        }

        /// <summary>
        /// Inserts a new synoptic layout
        /// </summary>
        /// <param name="synopticLayout">The synoptic layout to insert</param>
        /// <returns>The result of the operation</returns>
        [HttpGet]
        [Route("InsertSynoptic")]
        public async Task<IHttpActionResult> InsertSynoptic([FromBody] SynopticLayout synopticLayout)
        {
            if (!ModelState.IsValid)
            {
                var errorResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
                errorResponse.Content = new StringContent(
                    JsonConvert.SerializeObject(new
                    {
                        ErrorList = new List<object> { new { Description = "Invalid model state" } }
                    }),
                    Encoding.UTF8,
                    "application/json"
                );

                AddCorsHeaders(errorResponse);
                return ResponseMessage(errorResponse);
            }

            if (synopticLayout == null)
            {
                var errorResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
                errorResponse.Content = new StringContent(
                    JsonConvert.SerializeObject(new
                    {
                        ErrorList = new List<object> { new { Description = "Synoptic layout data is required" } }
                    }),
                    Encoding.UTF8,
                    "application/json"
                );

                AddCorsHeaders(errorResponse);
                return ResponseMessage(errorResponse);
            }

            try
            {
                bool result = await _repository.InsertSynopticAsync(synopticLayout);
                var response = new HttpResponseMessage(result ? HttpStatusCode.Created : HttpStatusCode.Conflict);

                if (result)
                {
                    response.Headers.Location = new Uri(Request.RequestUri, $"GetSynoptic?layout={synopticLayout.Layout}");
                    response.Content = new StringContent(
                        JsonConvert.SerializeObject(new
                        {
                            SynopticList = new List<object> { synopticLayout },
                            ErrorList = new List<object>()
                        }),
                        Encoding.UTF8,
                        "application/json"
                    );
                }
                else
                {
                    response.Content = new StringContent(
                        JsonConvert.SerializeObject(new
                        {
                            ErrorList = new List<object> { new { Description = "A synoptic with this layout already exists" } }
                        }),
                        Encoding.UTF8,
                        "application/json"
                    );
                }

                AddCorsHeaders(response);
                return ResponseMessage(response);
            }
            catch (Exception ex)
            {
                var errorResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                errorResponse.Content = new StringContent(
                    JsonConvert.SerializeObject(new
                    {
                        ErrorList = new List<object> { new { Description = ex.Message } }
                    }),
                    Encoding.UTF8,
                    "application/json"
                );

                AddCorsHeaders(errorResponse);
                return ResponseMessage(errorResponse);
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
                var errorResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
                errorResponse.Content = new StringContent(
                    JsonConvert.SerializeObject(new
                    {
                        ErrorList = new List<object> { new { Description = "Invalid model state" } }
                    }),
                    Encoding.UTF8,
                    "application/json"
                );

                AddCorsHeaders(errorResponse);
                return ResponseMessage(errorResponse);
            }

            if (synopticLayout == null)
            {
                var errorResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
                errorResponse.Content = new StringContent(
                    JsonConvert.SerializeObject(new
                    {
                        ErrorList = new List<object> { new { Description = "Synoptic layout data is required" } }
                    }),
                    Encoding.UTF8,
                    "application/json"
                );

                AddCorsHeaders(errorResponse);
                return ResponseMessage(errorResponse);
            }

            try
            {
                bool result = await _repository.UpdateSynopticAsync(synopticLayout);
                var response = new HttpResponseMessage(result ? HttpStatusCode.OK : HttpStatusCode.NotFound);

                if (result)
                {
                    response.Content = new StringContent(
                        JsonConvert.SerializeObject(new
                        {
                            SynopticList = new List<object> { synopticLayout },
                            ErrorList = new List<object>()
                        }),
                        Encoding.UTF8,
                        "application/json"
                    );
                }
                else
                {
                    response.Content = new StringContent(
                        JsonConvert.SerializeObject(new
                        {
                            ErrorList = new List<object> { new { Description = "Synoptic not found" } }
                        }),
                        Encoding.UTF8,
                        "application/json"
                    );
                }

                AddCorsHeaders(response);
                return ResponseMessage(response);
            }
            catch (Exception ex)
            {
                var errorResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                errorResponse.Content = new StringContent(
                    JsonConvert.SerializeObject(new
                    {
                        ErrorList = new List<object> { new { Description = ex.Message } }
                    }),
                    Encoding.UTF8,
                    "application/json"
                );

                AddCorsHeaders(errorResponse);
                return ResponseMessage(errorResponse);
            }
        }

        // Metodo helper per aggiungere gli header CORS
        private void AddCorsHeaders(HttpResponseMessage response)
        {
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, OPTIONS");
            response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Accept");
            response.Headers.Add("Access-Control-Max-Age", "86400");
        }

        // Aggiungi metodi OPTIONS per gli altri endpoint
        [HttpOptions]
        [Route("GetSynoptic")]
        public IHttpActionResult GetSynopticOptions()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            AddCorsHeaders(response);
            return ResponseMessage(response);
        }

        [HttpOptions]
        [Route("InsertSynoptic")]
        public IHttpActionResult InsertSynopticOptions()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            AddCorsHeaders(response);
            return ResponseMessage(response);
        }

        [HttpOptions]
        [Route("UpdateSynoptic")]
        public IHttpActionResult UpdateSynopticOptions()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            AddCorsHeaders(response);
            return ResponseMessage(response);
        }
    }
}