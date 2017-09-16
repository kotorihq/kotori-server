using Microsoft.AspNetCore.Authorization;
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
        KotoriCore.Configuration.Kotori _kotori;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:KotoriServer.Controllers.InstanceController"/> class.
        /// </summary>
        /// <param name="config">Config.</param>
        public InstanceController(IConfiguration config)
        {
            _kotori = config.ToKotoriConfiguration();
        }

        /// <summary>
        /// Get instance name.
        /// </summary>
        /// <returns>The instance name.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(string), 200)]
        [Authorize("master")]
        public string Get()
        {
            return _kotori.DocumentDb.EndPoint;
        }
    }
}
