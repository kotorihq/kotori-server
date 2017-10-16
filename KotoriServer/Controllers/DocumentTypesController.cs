using KotoriCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace KotoriServer.Controllers
{
    [Route("api/projects/{projectId}/document-types")]
    public class DocumentTypesController
    {
        readonly Kotori _kotori;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:KotoriServer.Controllers.DocumentTypesController"/> class.
        /// </summary>
        /// <param name="config">Config.</param>
        public DocumentTypesController(IConfiguration config, IHttpContextAccessor contextAccessor)
        {
            _kotori = new Kotori(config);
        }

        [HttpGet]
        [ProducesResponseType(typeof(string), 200)]
        public string Get()
        {
            // TODO: test
            return _kotori.Configuration.Instance;
        }
    }
}
