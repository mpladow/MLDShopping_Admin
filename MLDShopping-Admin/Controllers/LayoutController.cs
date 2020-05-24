using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MLDShopping_Admin.Controllers
{
    public class LayoutController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
        //public ActionResult GetFirstName()
        //{

        //    var firstName = ((ClaimsIdentity)User.Identity).Claims.FirstOrDefault(c => c.Type == "FirstName").Value;
        //    if (string.IsNullOrEmpty(firstName))
        //        return firstName;
        //    else
        //        return String.Empty;

        //}
        //public ActionResult GetLastName()
        //{
        //    var firstName = ((ClaimsIdentity)User.Identity).Claims.FirstOrDefault(c => c.Type == "LastName").Value;
        //    if (string.IsNullOrEmpty(firstName))
        //        return firstName;
        //    else
        //        return String.Empty;
        //}

        public string GetUserImageUrl()
        {
            var email = ((ClaimsIdentity)User.Identity).Claims.FirstOrDefault(c => c.Type == "UserImage").Value;
            if (string.IsNullOrEmpty(email))
                return email;
            else
                return String.Empty;
        }
    }
}
