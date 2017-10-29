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
        [Route("document-types/{documentTypeId}")]
        [HttpGet]
        [ProducesResponseType(typeof(SimpleDocumentType), 200)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<SimpleDocumentType> GetDocumentType(string projectId, string documentTypeId)
        {
            var documentType = await _kotori.GetDocumentTypeAsync(_instance, projectId, documentTypeId);

            return documentType;
        }

        [Authorize("readonlyproject")]
        [Route("document-types/{documentTypeId}/count")]
        [HttpGet]
        [ProducesResponseType(typeof(CountResult), 200)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<CountResult> CountDocuments(string projectId, string documentTypeId, [FromQuery]string filter, [FromQuery]bool drafts = false, [FromQuery]bool future = false)
        {
            var count = await _kotori.CountDocumentsAsync(_instance, projectId, documentTypeId, filter, drafts, future);

            return new CountResult(count);
        }

        [Authorize("readonlyproject")]
        [Route("document-types/{documentTypeId}/documents/{documentId}/{index?}")]
        [HttpGet]
        [ProducesResponseType(typeof(SimpleDocument), 200)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<SimpleDocument> GetDocument(string projectId, string documentTypeId, string documentId, string index, [FromQuery]long? version, [FromQuery]string format)
        {
            var df = KotoriCore.Helpers.Enums.DocumentFormat.Markdown;

            if (!string.IsNullOrEmpty(format) &&
               !format.Equals("html", System.StringComparison.OrdinalIgnoreCase))
                df = KotoriCore.Helpers.Enums.DocumentFormat.Markdown;

            var document = await _kotori.GetDocumentAsync(_instance, projectId, (documentTypeId ?? "") + "/" + documentId + (index != null ? "?" + index : ""), version, df);

            return document;
        }

        [Authorize("readonlyproject")]
        [Route("document-types/{documentTypeId}/documents/{documentId}")]
        [HttpGet]
        [ProducesResponseType(typeof(SimpleDocument), 200)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<SimpleDocument> GetDocument(string projectId, string documentTypeId, string documentId, [FromQuery]long? version, [FromQuery]string format)
        {
            var df = KotoriCore.Helpers.Enums.DocumentFormat.Markdown;

            if (!string.IsNullOrEmpty(format) &&
               !format.Equals("html", System.StringComparison.OrdinalIgnoreCase))
                df = KotoriCore.Helpers.Enums.DocumentFormat.Markdown;

            var document = await _kotori.GetDocumentAsync(_instance, projectId, (documentTypeId ?? "") + "/" + documentId, version, df);

            return document;
        }

        [Authorize("readonlyproject")]
        [Route("document-types/{documentTypeId}/documents/{documentId}/versions")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SimpleDocumentVersion>), 200)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IEnumerable<SimpleDocumentVersion>> GetDocumentVersions(string projectId, string documentTypeId, string documentId)
        {
            var documentVersions = await _kotori.GetDocumentVersionsAsync(_instance, projectId, (documentTypeId ?? "") + "/" + documentId);

            return documentVersions;
        }

        [Authorize("readonlyproject")]
        [Route("document-types/{documentTypeId}/documents/{documentId}/{index}/versions")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SimpleDocumentVersion>), 200)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IEnumerable<SimpleDocumentVersion>> GetDocumentVersions(string projectId, string documentTypeId, string documentId, string index)
        {
            var documentVersions = await _kotori.GetDocumentVersionsAsync(_instance, projectId, (documentTypeId ?? "") + "/" + documentId + (index != null ? "?" + index : ""));

            return documentVersions;
        }

        [Authorize("project")]
        [Route("document-types/{documentTypeId}/documents/{documentId}/{index?}")]
        [HttpDelete]
        [ProducesResponseType(typeof(CountResult), 200)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<string> DeleteDocument(string projectId, string documentTypeId, string documentId, string index)
        {
            var result = await _kotori.DeleteDocumentAsync(_instance, projectId, (documentTypeId ?? "") + "/" + documentId + (index != null ? "?" + index : ""));

            return result;
        }

        [Authorize("project")]
        [Route("document-types/{documentTypeId}/documents/{documentId}")]
        [HttpDelete]
        [ProducesResponseType(typeof(CountResult), 200)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<string> DeleteDocument(string projectId, string documentTypeId, string documentId)
        {
            var result = await _kotori.DeleteDocumentAsync(_instance, projectId, (documentTypeId ?? "") + "/" + documentId);

            return result;
        }

        [Authorize("readonlyproject")]
        [Route("document-types/{documentTypeId}/find")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SimpleDocument>), 200)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IEnumerable<SimpleDocument>> FindDocuments(string projectId, string documentTypeId, [FromQuery]int? top, [FromQuery]string select,
                                                [FromQuery]string filter, [FromQuery]string orderBy, [FromQuery]bool drafts = false,
                                                [FromQuery]bool future = false, [FromQuery]int? skip = null, [FromQuery]string format = null)
        {
            var df = KotoriCore.Helpers.Enums.DocumentFormat.Markdown;

            if (!string.IsNullOrEmpty(format) &&
               !format.Equals("html", System.StringComparison.OrdinalIgnoreCase))
                df = KotoriCore.Helpers.Enums.DocumentFormat.Markdown;

            var documents = await _kotori.FindDocumentsAsync(_instance, projectId, documentTypeId, top, select, filter, orderBy, drafts, future, skip, df);

            return documents;
        }
    }
}