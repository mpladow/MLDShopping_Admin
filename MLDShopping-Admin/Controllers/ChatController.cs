using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MLDShopping_Admin.Models;

namespace MLDShopping_Admin.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult Index()
        {
            var user = this.User;
            var model = new ChatVM();

            model.Name= user.FindFirst(ClaimTypes.Name).Value;
            model.Email = user.FindFirst(ClaimTypes.Email).Value;
            return View(model);
        }
    }
}