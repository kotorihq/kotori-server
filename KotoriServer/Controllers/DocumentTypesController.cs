using System.Collections.Generic;
using System.Threading.Tasks;
using KotoriCore;
using KotoriCore.Domains;
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
        public DocumentTypesController(IConfiguration config)
        {
            _kotori = new Kotori(config);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SimpleDocumentType>), 200)]
        public async Task<IEnumerable<SimpleDocumentType>> Get(string projectId)
        {
            var documentTypes = await _kotori.GetDocumentTypesAsync(_kotori.Configuration.Instance, projectId);

            return documentTypes;
        }
    }
}
