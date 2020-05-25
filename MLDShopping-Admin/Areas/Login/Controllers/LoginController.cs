using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MLDShopping_Admin.Entities;
using MLDShopping_Admin.Interfaces;
using MLDShopping_Admin.Services;

namespace MLDShopping_Admin.Controllers
{
    [Area("Login")]
    public class LoginController : Controller
    {
        public IAuthentication _auth { get; set; }
        public IAzureBlobService _azureBlobService;

        public LoginController(IAuthentication auth, IAzureBlobService azureBlobService)
        {
            _auth = auth;
            _azureBlobService = azureBlobService;

        }
        [HttpGet]
        public IActionResult Index(string message)
        {
            ViewData["Message"] = message;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(string username, string password, bool rememberMe)
        {
            var accountFromService = _auth.Authenticate(username, password);
            if (accountFromService == null)
            {
                return RedirectToAction("Index", new { message = "Your details were incorrect." });
            }
            var userClaims = new List<Claim>()
                {
                    new Claim("FirstName", accountFromService.FirstName),
                    new Claim("LastName", accountFromService.LastName),
                    new Claim("Email", accountFromService.Email)
                };
            if (accountFromService.UserImageUrl != null)
            {
                var fileUrl = await _azureBlobService.GetUriByNameAsync(accountFromService.UserImageUrl, "assets");
                Set("UserImage", fileUrl.AbsoluteUri, null);
            }
            //var accountClaims = new List<Claim>()
            //    {
            //        new Claim(ClaimTypes.Name, "Michael"),
            //        new Claim(ClaimTypes.Email, "email@email.com")
            //    };
            var permissionsClaims = new List<Claim>();
            accountFromService.Permissions.ForEach(p =>
            {
                var claim = new Claim(ClaimTypes.Role, p.Name);
                permissionsClaims.Add(claim);
            });
            var identity = new ClaimsIdentity(userClaims, "User");
            //var account = new ClaimsIdentity(accountClaims, "Account");
            var roles = new ClaimsIdentity(permissionsClaims, "Roles");

            var accountPrincipal = new ClaimsPrincipal(new[] { identity, roles });

            // set cookies here
            HttpContext.SignInAsync(accountPrincipal);
            return RedirectToAction("Index", "Home", new { Area= "Dashboard" });
        }

        /// <summary>  
        /// set the cookie  
        /// </summary>  
        /// <param name="key">key (unique indentifier)</param>  
        /// <param name="value">value to store in cookie object</param>  
        /// <param name="expireTime">expiration time</param>  
        public void Set(string key, string value, int? expireTime)
        {
            CookieOptions option = new CookieOptions();

            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddMinutes(expireTime.Value);

            Response.Cookies.Append(key, value, option);
        }
        /// <summary>  
        /// Delete the key  
        /// </summary>  
        /// <param name="key">Key</param>  
        public void Remove(string key)
        {
            Response.Cookies.Delete(key);
        }
    }
}