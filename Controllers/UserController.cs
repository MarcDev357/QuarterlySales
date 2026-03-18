using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace QuarterlySales.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View(_userManager.Users);
        }

        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user != null && user.UserName != "admin")
            {
                await _userManager.DeleteAsync(user);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> AddToAdmin(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user != null && !await _userManager.IsInRoleAsync(user, "Admin"))
            {
                await _userManager.AddToRoleAsync(user, "Admin");
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> RemoveFromAdmin(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user != null && user.UserName != "admin" && await _userManager.IsInRoleAsync(user, "Admin"))
            {
                await _userManager.RemoveFromRoleAsync(user, "Admin");
            }

            return RedirectToAction("Index");
        }
    }
}