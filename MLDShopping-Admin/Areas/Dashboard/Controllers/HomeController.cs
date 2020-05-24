using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MLDShopping_Admin.Components;
using MLDShopping_Admin.Models;

namespace MLDShopping_Admin.Controllers
{
    [Area("Dashboard")]
    [Authorize]
    [BreadcrumbActionFilter]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var myView = View();
            ViewBag.Title = "Dashboard";
            ViewData["PageHeader"] = "Home";
            ViewData["Description"] = "Do nothing here";
            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
