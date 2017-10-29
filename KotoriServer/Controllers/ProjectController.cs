using System.Threading.Tasks;
using KotoriCore;
using KotoriCore.Domains;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using KotoriServer.Tokens;

namespace KotoriServer.Controllers
{
    /// <summary>
    /// Project controller.
    /// </summary>
    [Route("api/projects/{projectId}")]
    public class ProjectController
    {
        readonly Kotori _kotori;
        readonly string _instance;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:KotoriServer.Controllers.ProjectController"/> class.
        /// </summary>
        /// <param name="kotori">Kotori.</param>
        public ProjectController(IKotori kotori)
        {
            _kotori = kotori as Kotori;
            _instance = kotori.Configuration.Instance;
        }

        [Authorize("readonlyproject")]
        [Route("document-types")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SimpleDocumentType>), 200)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IEnumerable<SimpleDocumentType>> GetDocumentTypes(string projectId)
        {
            var documentTypes = await _kotori.GetDocumentTypesAsync(_instance, projectId);

            return documentTypes;
        }

        [Authorize("readonlyproject")]
        [Route("document-types/{documentTypeId}/count")]
        [HttpGet]
        [ProducesResponseType(typeof(CountResult), 200)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<CountResult> CountDocuments(string projectId, string documentTypeId, [FromQuery]string filter, [FromQuery]bool drafts, [FromQuery]bool future)
        {
            var count = await _kotori.CountDocumentsAsync(_instance, projectId, documentTypeId, filter, drafts, future);

            return new CountResult(count);
        }

        [Authorize("project")]
        [Route("document-types/{documentTypeId}/{documentId}/{index?}")]
        [HttpGet]
        [ProducesResponseType(typeof(CountResult), 200)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<string> DeleteDocument(string projectId, string documentTypeId, string documentId, string index)
        {
            var result = await _kotori.DeleteDocumentAsync(_instance, projectId, (documentTypeId ?? "") + "/" + documentId + (index != null ? "?" + index : ""));

            return result;
        }
    }
}