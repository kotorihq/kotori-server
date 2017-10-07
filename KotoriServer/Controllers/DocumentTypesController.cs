using System;
namespace KotoriServer.Controllers
{
    [Route("api/projects/{projectId}")]
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
        [Authorize("")]
        public async Task<SimpleProject> Get(string projectId)
        {
            var result = await _kotori.GetProjectsAsync(_kotori.Configuration.Instance);

            var project = result.FirstOrDefault(p => p.Identifier.Equals(projectId));

            if (project == null)
                throw new KotoriProjectException(projectId, "Project not found.");

            return project;
        }
    }
}
