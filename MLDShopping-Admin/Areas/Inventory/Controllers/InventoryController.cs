using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MLDShopping_Admin.Areas.Inventory.Controllers
{
    [Area("Inventory")]
    public class InventoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}