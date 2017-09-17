using KotoriServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using KotoriCore;

namespace KotoriServer.Controllers
{
    public class SiteController : Controller
    {
        Kotori _kotori;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:KotoriServer.Controllers.PresentationController"/> class.
        /// </summary>
        /// <param name="config">Config.</param>
        public SiteController(IConfiguration config)
        {
            _kotori = new Kotori(config);
        }

        [HttpGet]                
        public IActionResult Index()
        {            
            return View(new SiteIndex(_kotori));            
        }
        
        public IActionResult Error()
        {
            return View(new SiteError { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
