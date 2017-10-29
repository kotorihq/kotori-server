using System.Collections.Generic;
using System.Threading.Tasks;
using KotoriCore;
using KotoriCore.Domains;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace KotoriServer.Controllers
{
    [Route("api")]
    [Authorize("master")]
    public class MasterController
    {
        readonly Kotori _kotori;

        public MasterController(IConfiguration config)
        {
            _kotori = new Kotori(config);
        }

        [Route("projects")]
        [HttpPost]
        [ProducesResponseType(typeof(string), 202)]
        public async Task<string> Post(string name, string identifier, [FromBody]IEnumerable<KotoriCore.Configurations.ProjectKey> projectKeys = null)
        {
            var result = await _kotori.CreateProjectAsync(_kotori.Configuration.Instance, name, identifier, projectKeys);
            return result;
        }

        [Route("projects")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SimpleProject>), 200)]
        public async Task<IEnumerable<SimpleProject>> Get()
        {
            var result = await _kotori.GetProjectsAsync(_kotori.Configuration.Instance);
            return result;
        }

        [Route("projects/{projectId}/project-keys/{key}")]
        [HttpPost]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<string> PostProjectKey(string projectId, string key, [FromQuery]bool isReadonly = false)
        {
            var result = await _kotori.CreateProjectKeyAsync(_kotori.Configuration.Instance, projectId, new KotoriCore.Configurations.ProjectKey { Key = key, IsReadonly = isReadonly });

            return result;
        }
    }
}
