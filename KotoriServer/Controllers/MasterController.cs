using System.Collections.Generic;
using System.Threading.Tasks;
using KotoriCore;
using KotoriCore.Domains;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KotoriServer.Controllers
{
    [Route("api")]
    [Authorize("master")]
    public class MasterController
    {
        readonly Kotori _kotori;
        readonly string _instance;

        public MasterController(IKotori kotori)
        {
            _kotori = kotori as Kotori;
            _instance = kotori.Configuration.Instance;
        }

        [Route("projects")]
        [HttpPost]
        [ProducesResponseType(typeof(string), 202)]
        public async Task<string> Post(string name, string identifier, [FromBody]IEnumerable<KotoriCore.Configurations.ProjectKey> projectKeys = null)
        {
            var result = await _kotori.CreateProjectAsync(_instance, name, identifier, projectKeys);
            return result;
        }

        [Route("projects")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SimpleProject>), 200)]
        public async Task<IEnumerable<SimpleProject>> Get()
        {
            var result = await _kotori.GetProjectsAsync(_instance);
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
