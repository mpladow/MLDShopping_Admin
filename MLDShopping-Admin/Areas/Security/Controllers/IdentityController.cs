using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace MLDShopping_Admin.Controllers
{
    [Area("Security")]
    public class IdentityController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Login", new { area= "Login"});
        }
        public async Task<IActionResult> AccessDenied()
        {
            return View();
        }
    }
}