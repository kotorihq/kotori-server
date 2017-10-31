using KotoriCore;
using KotoriServer.Examples;
using KotoriServer.Tokens;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Examples;

namespace KotoriServer.Controllers
{
    /// <summary>
    /// Instance controller.
    /// </summary>
    [Route("api/instance")]
    public class InstanceController
    {
        readonly Kotori _kotori;
        readonly string _instance;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:KotoriServer.Controllers.InstanceController"/> class.
        /// </summary>
        /// <param name="kotori">Kotori.</param>
        public InstanceController(IKotori kotori)
        {
            _kotori = kotori as Kotori;
            _instance = _kotori.Configuration.Instance;
        }

        /// <summary>
        /// Get instance information
        /// </summary>
        /// <returns>The instance information</returns>
        /// <response code="200">Instance information</response>
        /// <remarks>Just a simple method for testing purposes. It returns information about instance which you already know :)</remarks>
        [HttpGet]
        [ProducesResponseType(typeof(InstanceResult), 200)]
        [SwaggerResponseExample(200, typeof(InstanceExample))]
        public InstanceResult Get()
        {
            return new InstanceResult(_instance);
        }
    }
}
