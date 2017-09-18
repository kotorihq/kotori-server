using KotoriCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace KotoriServer.Controllers
{
    /// <summary>
    /// Instance controller.
    /// </summary>
    [Route("api/instance")]
    public class InstanceController
    {
        Kotori _kotori;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:KotoriServer.Controllers.InstanceController"/> class.
        /// </summary>
        /// <param name="config">Config.</param>
        public InstanceController(IConfiguration config)
        {
            _kotori = new Kotori(config);
        }

        /// <summary>
        /// Get instance name.
        /// </summary>
        /// <returns>The instance name.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(string), 200)]
        public string Get()
        {
            // TODO: command in kotori core
            return _kotori.Configuration.Instance;
        }
    }
}
