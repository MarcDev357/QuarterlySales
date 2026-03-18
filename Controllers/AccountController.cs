using Microsoft.AspNetCore.Mvc;

namespace QuarterlySales.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Register()
        {
            return Redirect("/Identity/Account/Register");
        }
    }
}