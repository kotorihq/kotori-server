using Microsoft.AspNetCore.Mvc;

namespace KotoriServer.Controllers
{
    /// <summary>
    /// Instance controller.
    /// </summary>
    [Route("api/instance")]
    public class InstanceController : BaseController
    {
		[HttpGet]
		[ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(void), 401)]
		[ProducesResponseType(typeof(void), 500)]
		public string Get()
		{
            return "???";
		}
    }
}
