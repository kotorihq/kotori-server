using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace KotoriServer.Controllers
{
    public class BaseController : Controller
    {
        IConfiguration _config;

        public BaseController(IConfiguration config)
        {
            _config = config;
        }

        public BaseController()
        {
        }
    }
}
