using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MoviesDashboard.Data;
using MoviesDashboard.ViewModels;

namespace MoviesDashboard.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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

        [HttpGet]
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
                ModelState.AddModelError(string.Empty, "Invalid User_Name/Email or Password");
                return View(vm);
            }


            var pass = await _signInManager.PasswordSignInAsync(user, vm.Password, vm.RememberMe, true);

            if (!pass.Succeeded)
            {
                if (pass.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty, "Too Many Attempt Please try again after 5 min");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid User_Name/Email or Password");
                }
                return View(vm);
            }
            TempData["Success-notification"] = "Login Successfully";
            return RedirectToAction(actionName: "Index", controllerName: "Home", new { area = "Admin" });

        }


        public IActionResult ForgotPassword()
        {
            return View();
        }

    }
}
