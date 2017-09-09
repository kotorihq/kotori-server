using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace KotoriServer.Controllers
{
    /// <summary>
    /// Base controller.
    /// </summary>
    public class BaseController : Controller
    {
        IConfiguration _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:KotoriServer.Controllers.BaseController"/> class.
        /// </summary>
        /// <param name="config">Config.</param>
        public BaseController(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:KotoriServer.Controllers.BaseController"/> class.
        /// </summary>
        public BaseController()
        {
        }
    }
}
