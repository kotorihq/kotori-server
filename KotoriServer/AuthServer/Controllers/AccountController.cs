using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IdentityServer4;
using KotoriServer.Helpers;

namespace KotoriServer.AuthServer.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("account")]
    public class AccountController : Controller
    {
        readonly LoginService _loginService;

        public AccountController(LoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpGet("login")]
        public IActionResult Login(string returnUrl)
        {
            var viewModel = new LoginViewModel { ReturnUrl = returnUrl };

            return View("/AuthServer/Views/Login.cshtml", viewModel);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm]LoginViewModel viewModel)
        {
            if (!_loginService.ValidateCredentials(viewModel.Key, viewModel.Type))
            {
                ModelState.AddModelError("", "Invalid username or password");
                return View("/AuthServer/Views/Login.cshtml", viewModel);
            }

            // Use an IdentityServer-compatible ClaimsPrincipal
            var principal = IdentityServerPrincipal.Create(viewModel.Key, viewModel.Type.ToString());
            await HttpContext.Authentication.SignInAsync("Cookies", principal);

            return Redirect(viewModel.ReturnUrl);
        }
    }

    public class LoginViewModel
    {
        public string ReturnUrl { get; set; }
        public string Key { get; set; }
        public Enums.KeyUserType Type { get; set; }
    }
}
