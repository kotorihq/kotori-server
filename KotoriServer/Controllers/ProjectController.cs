using System.Collections.Generic;
using KotoriCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using KotoriCore.Domains;
using KotoriServer.Tokens;
using Microsoft.AspNetCore.Cors;

namespace KotoriServer.Controllers
{
    /// <summary>
    /// Project controller.
    /// </summary>
    [EnableCors("AllowAnyOrigin")]
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
        [Route("document-types/{documentType}/{documentTypeId}")]
        [HttpGet]
        [ProducesResponseType(typeof(SimpleDocumentType), 200)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<SimpleDocumentType> GetDocumentType(string projectId, string documentType, string documentTypeId)
        {
            var docType = await _kotori.GetDocumentTypeAsync(_instance, projectId, documentType + "/" + documentTypeId);

            return docType;
        }

        [Authorize("readonlyproject")]
        [Route("document-types/{documentType}/{documentTypeId}/count")]
        [HttpGet]
        [ProducesResponseType(typeof(CountResult), 200)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<CountResult> CountDocuments(string projectId, string documentType, string documentTypeId, 
                                                      [FromQuery]string filter, [FromQuery]bool drafts = false, [FromQuery]bool future = false)
        {
            var count = await _kotori.CountDocumentsAsync(_instance, projectId, documentType + "/" + documentTypeId, filter, drafts, future);

            return new CountResult(count);
        }

        [Authorize("readonlyproject")]
        [Route("document-types/{documentType}/{documentTypeId}/documents/{documentId}/{index:long}/versions/{version:long}")]
        [HttpGet]
        [ProducesResponseType(typeof(SimpleDocument), 200)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<SimpleDocument> GetDocument(string projectId, string documentType, string documentTypeId,
                                                      string documentId, long index, long version, [FromQuery]string format)
        {
            var df = KotoriCore.Helpers.Enums.DocumentFormat.Markdown;

            if (!string.IsNullOrEmpty(format) &&
               !format.Equals("html", System.StringComparison.OrdinalIgnoreCase))
                df = KotoriCore.Helpers.Enums.DocumentFormat.Markdown;

            var document = await _kotori.GetDocumentAsync
            (
                _instance, 
                projectId, 
                documentType + "/" + documentTypeId + "/" + documentId + "?" + index,
                version, 
                df
            );

            return document;
        }

        [Authorize("readonlyproject")]
        [Route("document-types/{documentType}/{documentTypeId}/documents/{documentId}/{index:long?}")]
        [HttpGet]
        [ProducesResponseType(typeof(SimpleDocument), 200)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<SimpleDocument> GetDocument(string projectId, string documentType, string documentTypeId, 
                                                      string documentId, long? index, [FromQuery]string format)
        {
            var df = KotoriCore.Helpers.Enums.DocumentFormat.Markdown;

            if (!string.IsNullOrEmpty(format) &&
               !format.Equals("html", System.StringComparison.OrdinalIgnoreCase))
                df = KotoriCore.Helpers.Enums.DocumentFormat.Markdown;

            var document = await _kotori.GetDocumentAsync
            (
                _instance, 
                projectId, 
                documentType + "/" + documentTypeId + "/" + documentId + (index != null ? "?" + index : ""),
                null, 
                df
            );

            return document;
        }

        [Authorize("readonlyproject")]
        [Route("document-types/{documentType}/{documentTypeId}/documents/{documentId}/versions")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SimpleDocumentVersion>), 200)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IEnumerable<SimpleDocumentVersion>> GetDocumentVersions(string projectId, string documentType,
                                                                                  string documentTypeId, string documentId)
        {
            var documentVersions = await _kotori.GetDocumentVersionsAsync
            (
                _instance, 
                projectId, 
                documentType + "/" + documentTypeId + "/" + documentId
            );

            return documentVersions;
        }

        [Authorize("readonlyproject")]
        [Route("document-types/{documentType}/{documentTypeId}/documents/{documentId}/{index:long}/versions")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SimpleDocumentVersion>), 200)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IEnumerable<SimpleDocumentVersion>> GetDocumentVersions(string projectId, string documentType,
                                                                                  string documentTypeId, string documentId, 
                                                                                  long index)
        {
            var documentVersions = await _kotori.GetDocumentVersionsAsync
            (
                _instance, 
                projectId, 
                documentType + "/" + documentTypeId + "/" + documentId + "?" + index
            );

            return documentVersions;
        }

        [Authorize("project")]
        [Route("document-types/{documentType}/{documentTypeId}/documents/{documentId}/{index:long?}")]
        [HttpDelete]
        [ProducesResponseType(typeof(CountResult), 200)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<string> DeleteDocument(string projectId, string documentType, string documentTypeId, string documentId, long? index)
        {
            var result = await _kotori.DeleteDocumentAsync
            (
                  _instance, 
                  projectId, 
                  documentType + "/" + documentTypeId + "/" + documentId + (index != null ? "?" + index : "")
            );

            return result;
        }

        [Authorize("readonlyproject")]
        [Route("document-types/{documentType}/{documentTypeId}/find")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SimpleDocument>), 200)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IEnumerable<SimpleDocument>> FindDocuments(string projectId, string documentType, string documentTypeId, [FromQuery]int? top, 
                                                [FromQuery]string select, [FromQuery]string filter, [FromQuery]string orderBy, [FromQuery]bool drafts = false,
                                                [FromQuery]bool future = false, [FromQuery]int? skip = null, [FromQuery]string format = null)
        {
            var df = KotoriCore.Helpers.Enums.DocumentFormat.Markdown;

            if (!string.IsNullOrEmpty(format) &&
               !format.Equals("html", System.StringComparison.OrdinalIgnoreCase))
                df = KotoriCore.Helpers.Enums.DocumentFormat.Markdown;

            var documents = await _kotori.FindDocumentsAsync(_instance, projectId, documentType + "/" + documentTypeId, top, select, filter, orderBy, drafts, future, skip, df);

            return documents;
        }

        [Authorize("project")]
        [Route("document-types/{documentType}/{documentTypeId}/documents/{documentId}/{index:long?}")]
        [HttpPost]
        [ProducesResponseType(typeof(string), 201)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<string> UpsertDocument(string projectId, string documentType, string documentTypeId,
                                                 string documentId, long? index, [FromBody]string content)
        {
            var result = await _kotori.UpsertDocumentAsync
            (
                _instance,
                projectId,
                documentType + "/" + documentTypeId + "/" + documentId + (index == null ? "" : "?" + index),
                content
            );

            return result;
        }
    }
}