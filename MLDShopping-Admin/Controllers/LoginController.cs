using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MLDShopping_Admin.Interfaces;

namespace MLDShopping_Admin.Controllers
{
    public class LoginController : Controller
    {
        public IAuthentication _auth { get; set; }

        public LoginController(IAuthentication auth)
        {
            _auth = auth;
        }
        [HttpGet]
        public IActionResult Index(string message)
        {
            ViewData["Message"] = message;
            return View();
        }
        [HttpPost]
        public IActionResult Index(string username, string password)
        {
            var accountFromService = _auth.Authenticate(username, password);
            if (accountFromService == null)
            {
                return RedirectToAction("Index", new { message= "Your details were incorrect." });
            }
            var userClaims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, "Michael"),
                    new Claim(ClaimTypes.Name, "La Dow")
                };
            var accountClaims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, "Michael"),
                    new Claim(ClaimTypes.Email, "email@email.com")
                };
            var permissionsClaims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Role, "Admin"),
                    new Claim(ClaimTypes.Role, "Manager")
                };
            var identity = new ClaimsIdentity(userClaims, "User");
            var account = new ClaimsIdentity(accountClaims, "Account");
            var roles = new ClaimsIdentity(permissionsClaims, "Roles");

            var accountPrincipal = new ClaimsPrincipal(new[] { identity, account, roles });
            HttpContext.SignInAsync(accountPrincipal);
            return RedirectToAction("Index", "Home");


        }
    }
}