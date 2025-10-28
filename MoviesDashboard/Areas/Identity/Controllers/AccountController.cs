using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MoviesDashboard.Data;
using MoviesDashboard.ViewModels;
using System.Threading.Tasks;

namespace MoviesDashboard.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public AccountController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var result = await _userManager.CreateAsync(new()
            {
                Name = vm.Name,
                Email = vm.Email,
                UserName = vm.UserName
            }, password: vm.Password);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Code);
                }
                return View(vm);
            }
            return RedirectToAction("Login");
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var user = await _userManager.FindByNameAsync(vm.UserNameOeEmail) ??
                await _userManager.FindByEmailAsync(vm.UserNameOeEmail);
            if (user is null)
            {

            }

            return View();
        }


        public IActionResult ForgotPassword()
        {
            return View();
        }

    }
}
