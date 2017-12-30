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
        [Route("content/{documentTypeId}")]
        [HttpGet]
        [ProducesResponseType(typeof(SimpleDocumentType), 200)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<SimpleDocumentType> GetContentDocumentType(string projectId, string documentTypeId)
        {
            var docType = await _kotori.GetDocumentTypeAsync(_instance, projectId, KotoriCore.Helpers.Enums.DocumentType.Content, documentTypeId);

            return docType;
        }

        [Authorize("readonlyproject")]
        [Route("data/{documentType}/{documentTypeId}")]
        [HttpGet]
        [ProducesResponseType(typeof(SimpleDocumentType), 200)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<SimpleDocumentType> GetDataDocumentType(string projectId, string documentTypeId)
        {
            var docType = await _kotori.GetDocumentTypeAsync(_instance, projectId, KotoriCore.Helpers.Enums.DocumentType.Data, documentTypeId);

            return docType;
        }

        [Authorize("readonlyproject")]
        [Route("content/{documentTypeId}/count")]
        [HttpGet]
        [ProducesResponseType(typeof(CountResult), 200)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<CountResult> CountContentDocuments(string projectId, string documentTypeId, 
                                                      [FromQuery]string filter, [FromQuery]bool drafts = false, [FromQuery]bool future = false)
        {
            var count = await _kotori.CountDocumentsAsync(_instance, projectId, KotoriCore.Helpers.Enums.DocumentType.Content, documentTypeId, filter, drafts, future);

            return new CountResult(count);
        }

        [Authorize("readonlyproject")]
        [Route("data/{documentTypeId}/count")]
        [HttpGet]
        [ProducesResponseType(typeof(CountResult), 200)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<CountResult> CountDataDocuments(string projectId, string documentTypeId,
                                                      [FromQuery]string filter, [FromQuery]bool drafts = false, [FromQuery]bool future = false)
        {
            var count = await _kotori.CountDocumentsAsync(_instance, projectId, KotoriCore.Helpers.Enums.DocumentType.Data, documentTypeId, filter, drafts, future);

            return new CountResult(count);
        }

        [Authorize("readonlyproject")]
        [Route("content/{documentTypeId}/documents/{documentId}/versions/{version:long}")]
        [HttpGet]
        [ProducesResponseType(typeof(SimpleDocument), 200)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<SimpleDocument> GetContentDocument(string projectId, string documentTypeId,
                                                      string documentId, long version, [FromQuery]string format)
        {
            var df = KotoriCore.Helpers.Enums.DocumentFormat.Markdown;

            if (!string.IsNullOrEmpty(format) &&
               !format.Equals("html", System.StringComparison.OrdinalIgnoreCase))
                df = KotoriCore.Helpers.Enums.DocumentFormat.Markdown;

            var document = await _kotori.GetDocumentAsync
            (
                _instance,
                projectId,
                KotoriCore.Helpers.Enums.DocumentType.Content,
                documentTypeId,
                documentId,
                null,
                version,
                df
            );

            return document;
        }

        [Authorize("readonlyproject")]
        [Route("data/{documentTypeId}/documents/{documentId}/{index:long}/versions/{version:long}")]
        [HttpGet]
        [ProducesResponseType(typeof(SimpleDocument), 200)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<SimpleDocument> GetContentDocument(string projectId, string documentTypeId,
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
                KotoriCore.Helpers.Enums.DocumentType.Data,
                documentTypeId,
                documentId,
                index,
                version,
                df
            );

            return document;
        }

        [Authorize("readonlyproject")]
        [Route("content/{documentTypeId}/documents/{documentId}")]
        [HttpGet]
        [ProducesResponseType(typeof(SimpleDocument), 200)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<SimpleDocument> GetContentDocument(string projectId, string documentTypeId,
                                                      string documentId, [FromQuery]string format)
        {
            var df = KotoriCore.Helpers.Enums.DocumentFormat.Markdown;

            if (!string.IsNullOrEmpty(format) &&
               !format.Equals("html", System.StringComparison.OrdinalIgnoreCase))
                df = KotoriCore.Helpers.Enums.DocumentFormat.Markdown;

            var document = await _kotori.GetDocumentAsync
            (
                _instance,
                projectId,
                KotoriCore.Helpers.Enums.DocumentType.Content,
                documentTypeId,
                documentId,
                null,
                null,
                df
            );

            return document;
        }

        [Authorize("readonlyproject")]
        [Route("data/{documentTypeId}/documents/{documentId}/{index:long}")]
        [HttpGet]
        [ProducesResponseType(typeof(SimpleDocument), 200)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<SimpleDocument> GetDataDocument(string projectId, string documentTypeId,
                                                          string documentId, long index)
        {
            var document = await _kotori.GetDocumentAsync
            (
                _instance,
                projectId,
                KotoriCore.Helpers.Enums.DocumentType.Data,
                documentTypeId,
                documentId,
                index,
                null
            );

            return document;
        }

        [Authorize("readonlyproject")]
        [Route("content/{documentTypeId}/documents/{documentId}/versions")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SimpleDocumentVersion>), 200)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IEnumerable<SimpleDocumentVersion>> GetContentDocumentVersions(string projectId, string documentTypeId, string documentId)
        {
            var documentVersions = await _kotori.GetDocumentVersionsAsync
            (
                _instance,
                projectId,
                KotoriCore.Helpers.Enums.DocumentType.Content,
                documentTypeId,
                documentId,
                null
            );

            return documentVersions;
        }

        [Authorize("readonlyproject")]
        [Route("data/{documentTypeId}/documents/{documentId}/{index:long}/versions")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SimpleDocumentVersion>), 200)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IEnumerable<SimpleDocumentVersion>> GetDataDocumentVersions(string projectId, string documentTypeId, string documentId, long index)
        {
            var documentVersions = await _kotori.GetDocumentVersionsAsync
            (
                _instance,
                projectId,
                KotoriCore.Helpers.Enums.DocumentType.Data,
                documentTypeId,
                documentId,
                index
            );

            return documentVersions;
        }

        [Authorize("project")]
        [Route("content/{documentTypeId}/documents/{documentId}")]
        [HttpDelete]
        [ProducesResponseType(typeof(CountResult), 200)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<string> DeleteContentDocument(string projectId, string documentTypeId, string documentId)
        {
            var result = await _kotori.DeleteDocumentAsync
            (
                  _instance,
                  projectId,
                  KotoriCore.Helpers.Enums.DocumentType.Content,
                  documentTypeId,
                  documentId,
                  null
            );

            return result;
        }

        [Authorize("project")]
        [Route("data/{documentTypeId}/documents/{documentId}/{index:long?}")]
        [HttpDelete]
        [ProducesResponseType(typeof(CountResult), 200)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<string> DeleteDataDocument(string projectId, string documentTypeId, string documentId, long? index)
        {
            var result = await _kotori.DeleteDocumentAsync
            (
                  _instance,
                  projectId,
                  KotoriCore.Helpers.Enums.DocumentType.Data,
                  documentTypeId,
                  documentId,
                  index
            );

            return result;
        }

        [Authorize("readonlyproject")]
        [Route("content/{documentTypeId}/find")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SimpleDocument>), 200)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IEnumerable<SimpleDocument>> FindContentDocuments(string projectId, string documentTypeId, [FromQuery]int? top, 
                                                [FromQuery]string select, [FromQuery]string filter, [FromQuery]string orderBy, [FromQuery]bool drafts = false,
                                                [FromQuery]bool future = false, [FromQuery]int? skip = null, [FromQuery]string format = null)
        {
            var df = KotoriCore.Helpers.Enums.DocumentFormat.Markdown;

            if (!string.IsNullOrEmpty(format) &&
               !format.Equals("html", System.StringComparison.OrdinalIgnoreCase))
                df = KotoriCore.Helpers.Enums.DocumentFormat.Markdown;

            var documents = await _kotori.FindDocumentsAsync
            (
                 _instance,
                 projectId,
                 KotoriCore.Helpers.Enums.DocumentType.Content,
                 documentTypeId,
                 top,
                 select,
                 filter,
                 orderBy,
                 drafts,
                 future,
                 skip,
                 df
            );

            return documents;
        }

        [Authorize("readonlyproject")]
        [Route("data/{documentTypeId}/find")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SimpleDocument>), 200)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IEnumerable<SimpleDocument>> FindDataDocuments(string projectId, string documentTypeId, [FromQuery]int? top,
                                                [FromQuery]string select, [FromQuery]string filter, [FromQuery]string orderBy, [FromQuery]bool drafts = false,
                                                [FromQuery]bool future = false, [FromQuery]int? skip = null)
        {
            var documents = await _kotori.FindDocumentsAsync
            (
                 _instance,
                 projectId,
                 KotoriCore.Helpers.Enums.DocumentType.Data,
                 documentTypeId,
                 top,
                 select,
                 filter,
                 orderBy,
                 drafts,
                 future,
                 skip,
                 KotoriCore.Helpers.Enums.DocumentFormat.Html
            );

            return documents;
        }

        [Authorize("project")]
        [Route("content/{documentTypeId}/documents/{documentId}/{index:long?}")]
        [HttpPost]
        [ProducesResponseType(typeof(string), 201)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<string> UpsertContentDocument(string projectId, string documentTypeId, string documentId,
                                                        [FromBody]string content, [FromQuery]string date, [FromQuery]bool? draft)
        {
            var dt = date.ToDateTime();

            var result = await _kotori.UpsertDocumentAsync
            (
                _instance,
                projectId,
                KotoriCore.Helpers.Enums.DocumentType.Content,
                documentTypeId,
                documentId,
                null,
                content,
                dt,
                draft
            );

            return result;
        }

        [Authorize("project")]
        [Route("data/{documentTypeId}/documents/{documentId}/{index:long?}")]
        [HttpPost]
        [ProducesResponseType(typeof(string), 201)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<string> UpsertDataDocument(string projectId, string documentTypeId, string documentId,
                                                     long? index, [FromBody]string content, [FromQuery]string date, [FromQuery]bool? draft)
        {
            var dt = date.ToDateTime();

            var result = await _kotori.UpsertDocumentAsync
            (
                _instance,
                projectId,
                KotoriCore.Helpers.Enums.DocumentType.Data,
                documentTypeId,
                documentId,
                index,
                content,
                dt,
                draft
            );

            return result;
        }
    }
}