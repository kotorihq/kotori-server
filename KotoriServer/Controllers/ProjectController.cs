using System.Collections.Generic;
using KotoriCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using KotoriCore.Domains;
using Microsoft.AspNetCore.Cors;
using KotoriServer.Tokens;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace KotoriServer.Controllers
{
    /// <summary>
    /// Project controller.
    /// </summary>
    [EnableCors("AllowAnyOrigin")]
    [Route("api/projects/{projectId}")]
    public class ProjectController : ControllerBase
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

        /// <summary>
        /// Creates the content document type.
        /// </summary>
        /// <returns>The operation result.</returns>
        /// <param name="projectId">Project identifier.</param>
        [Authorize("project")]
        [Route("content")]
        [HttpPost]
        public async Task<IActionResult> CreateContentDocumentType(string projectId)
        {
            var result = await _kotori.CreateDocumentTypeAsync
            (
                  _instance,
                  projectId,
                  KotoriCore.Helpers.Enums.DocumentType.Content,
                  null
            );

            return Created(result.Url, new { id = result.Id, url = result.Url });
        }

        /// <summary>
        /// Upserts the type of the content document.
        /// </summary>
        /// <returns>The operation result.</returns>
        /// <param name="projectId">Project identifier.</param>
        [Authorize("project")]
        [Route("content/{documentTypeId}")]
        [HttpPut]
        public async Task<IActionResult> UpsertContentDocumentType(string projectId, [Required]string documentTypeId)
        {
            var result = await _kotori.UpsertDocumentTypeAsync
            (
                  _instance,
                  projectId,
                  KotoriCore.Helpers.Enums.DocumentType.Content,
                  documentTypeId
            );

            if (result.NewResource)
                return Created(result.Url, new { id = result.Id, url = result.Url });

            return Ok();
        }

        /// <summary>
        /// Gets the content document types.
        /// </summary>
        /// <returns>The document types.</returns>
        /// <param name="projectId">Project identifier.</param>
        [Authorize("readonlyproject")]
        [Route("content")]
        [HttpGet]
        public async Task<Tokens.ComplexCountResult<DocumentTypeResult>> GetContentDocumentTypes(string projectId)
        {
            var documentTypes = await _kotori.GetDocumentTypesAsync(_instance, projectId);
             
            return new Tokens.ComplexCountResult<DocumentTypeResult>(documentTypes.Count, documentTypes.Items.Select(x => (DocumentTypeResult)x));
        }

        /// <summary>
        /// Gets the content document type.
        /// </summary>
        /// <returns>The operation result.</returns>
        /// <param name="projectId">Project identifier.</param>
        /// <param name="documentTypeId">Document type identifier.</param>
        [Authorize("readonlyproject")]
        [Route("content/document-types/{documentTypeId}")]
        [HttpGet]
        public async Task<DocumentTypeResult> GetContentDocumentType(string projectId, [Required]string documentTypeId)
        {
            var docType = await _kotori.GetDocumentTypeAsync(_instance, projectId, KotoriCore.Helpers.Enums.DocumentType.Content, documentTypeId);

            return docType;
        }

        // -------------------- TODO

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
        [ProducesResponseType(typeof(Tokens.CountResult), 200)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<Tokens.CountResult> CountContentDocuments(string projectId, string documentTypeId, 
                                                      [FromQuery]string filter, [FromQuery]bool drafts = false, [FromQuery]bool future = false)
        {
            var count = await _kotori.CountDocumentsAsync(_instance, projectId, KotoriCore.Helpers.Enums.DocumentType.Content, documentTypeId, filter, drafts, future);

            return new Tokens.CountResult(count.Count);
        }

        [Authorize("readonlyproject")]
        [Route("data/{documentTypeId}/count")]
        [HttpGet]
        [ProducesResponseType(typeof(Tokens.CountResult), 200)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<Tokens.CountResult> CountDataDocuments(string projectId, string documentTypeId,
                                                      [FromQuery]string filter, [FromQuery]bool drafts = false, [FromQuery]bool future = false)
        {
            var count = await _kotori.CountDocumentsAsync(_instance, projectId, KotoriCore.Helpers.Enums.DocumentType.Data, documentTypeId, filter, drafts, future);

            return new Tokens.CountResult(count.Count);
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

        //[Authorize("readonlyproject")]
        //[Route("content/{documentTypeId}/documents/{documentId}/versions")]
        //[HttpGet]
        //[ProducesResponseType(typeof(ComplexCountResult<SimpleDocumentVersion>), 200)]
        //[ProducesResponseType(typeof(string), 404)]
        //public async Task<ComplexCountResult<SimpleDocumentVersion>> GetContentDocumentVersions(string projectId, string documentTypeId, string documentId)
        //{
        //    var documentVersions = await _kotori.GetDocumentVersionsAsync
        //    (
        //        _instance,
        //        projectId,
        //        KotoriCore.Helpers.Enums.DocumentType.Content,
        //        documentTypeId,
        //        documentId,
        //        null
        //    );

        //    return documentVersions;
        //}

        //[Authorize("readonlyproject")]
        //[Route("data/{documentTypeId}/documents/{documentId}/{index:long}/versions")]
        //[HttpGet]
        //[ProducesResponseType(typeof(ComplexCountResult<SimpleDocumentVersion>), 200)]
        //[ProducesResponseType(typeof(string), 404)]
        //public async Task<ComplexCountResult<SimpleDocumentVersion>> GetDataDocumentVersions(string projectId, string documentTypeId, string documentId, long index)
        //{
        //    var documentVersions = await _kotori.GetDocumentVersionsAsync
        //    (
        //        _instance,
        //        projectId,
        //        KotoriCore.Helpers.Enums.DocumentType.Data,
        //        documentTypeId,
        //        documentId,
        //        index
        //    );

        //    return documentVersions;
        //}

        [Authorize("project")]
        [Route("content/{documentTypeId}/documents/{documentId}")]
        [HttpDelete]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<StatusCodeResult> DeleteContentDocument(string projectId, string documentTypeId, string documentId)
        {
            await _kotori.DeleteDocumentAsync
            (
                  _instance,
                  projectId,
                  KotoriCore.Helpers.Enums.DocumentType.Content,
                  documentTypeId,
                  documentId,
                  null
            );

            return NoContent();
        }

        [Authorize("project")]
        [Route("data/{documentTypeId}/documents/{documentId}/{index:long?}")]
        [HttpDelete]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<StatusCodeResult> DeleteDataDocument(string projectId, string documentTypeId, string documentId, long? index)
        {
            await _kotori.DeleteDocumentAsync
            (
                  _instance,
                  projectId,
                  KotoriCore.Helpers.Enums.DocumentType.Data,
                  documentTypeId,
                  documentId,
                  index
            );

            return NoContent();
        }

        //[Authorize("readonlyproject")]
        //[Route("content/{documentTypeId}/find")]
        //[HttpGet]
        //[ProducesResponseType(typeof(ComplexCountResult<SimpleDocument>), 200)]
        //[ProducesResponseType(typeof(string), 404)]
        //public async Task<ComplexCountResult<SimpleDocument>> FindContentDocuments(string projectId, string documentTypeId, [FromQuery]int? top, 
        //                                        [FromQuery]string select, [FromQuery]string filter, [FromQuery]string orderBy, [FromQuery]bool drafts = false,
        //                                        [FromQuery]bool future = false, [FromQuery]int? skip = null, [FromQuery]string format = null)
        //{
        //    var df = KotoriCore.Helpers.Enums.DocumentFormat.Markdown;

        //    if (!string.IsNullOrEmpty(format) &&
        //       !format.Equals("html", System.StringComparison.OrdinalIgnoreCase))
        //        df = KotoriCore.Helpers.Enums.DocumentFormat.Markdown;

        //    var documents = await _kotori.FindDocumentsAsync
        //    (
        //         _instance,
        //         projectId,
        //         KotoriCore.Helpers.Enums.DocumentType.Content,
        //         documentTypeId,
        //         top,
        //         select,
        //         filter,
        //         orderBy,
        //         drafts,
        //         future,
        //         skip,
        //         df
        //    );

        //    return documents;
        //}

        //[Authorize("readonlyproject")]
        //[Route("data/{documentTypeId}/find")]
        //[HttpGet]
        //[ProducesResponseType(typeof(ComplexCountResult<SimpleDocument>), 200)]
        //[ProducesResponseType(typeof(string), 404)]
        //public async Task<ComplexCountResult<SimpleDocument>> FindDataDocuments(string projectId, string documentTypeId, [FromQuery]int? top,
        //                                        [FromQuery]string select, [FromQuery]string filter, [FromQuery]string orderBy, [FromQuery]bool drafts = false,
        //                                        [FromQuery]bool future = false, [FromQuery]int? skip = null)
        //{
        //    var documents = await _kotori.FindDocumentsAsync
        //    (
        //         _instance,
        //         projectId,
        //         KotoriCore.Helpers.Enums.DocumentType.Data,
        //         documentTypeId,
        //         top,
        //         select,
        //         filter,
        //         orderBy,
        //         drafts,
        //         future,
        //         skip,
        //         KotoriCore.Helpers.Enums.DocumentFormat.Html
        //    );

        //    return documents;
        //}

        [Authorize("project")]
        [Route("content/{documentTypeId}/documents/{documentId}/{index:long?}")]
        [HttpPost]
        [ProducesResponseType(typeof(string), 201)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IActionResult> UpsertContentDocument(string projectId, string documentTypeId, string documentId,
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

            return Created(result.Url, result);
        }

        [Authorize("project")]
        [Route("data/{documentTypeId}/documents/{documentId}/{index:long?}")]
        [HttpPost]
        [ProducesResponseType(typeof(string), 201)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IActionResult> UpsertDataDocument(string projectId, string documentTypeId, string documentId,
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

            return Created(result.Url, result);
        }
    }
}