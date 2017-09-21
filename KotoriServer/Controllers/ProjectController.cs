using System.Collections.Generic;
using System.Linq;
using KotoriCore;
using KotoriCore.Commands;
using KotoriCore.Configurations;
using KotoriCore.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace KotoriServer.Controllers
{
    /// <summary>
    /// Project controller.
    /// </summary>
    [Route("api/project")]
    public class ProjectController
    {
        Kotori _kotori;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:KotoriServer.Controllers.ProjectController"/> class.
        /// </summary>
        /// <param name="config">Config.</param>
        public ProjectController(IConfiguration config)
        {
            _kotori = new Kotori(config);
        }

		[HttpPost]
        [ProducesResponseType(typeof(void), 200)]
        [Authorize("master")]
		public void Post(string instance, string name, string identifier, [FromBody]List<ProjectKey> projectKeys)
		{
            var result = _kotori.Process(new CreateProject(instance, name, identifier, projectKeys));

            if (result.Any(r => !r.IsValid))
                throw new KotoriException("Damn");
		}
    }
}
