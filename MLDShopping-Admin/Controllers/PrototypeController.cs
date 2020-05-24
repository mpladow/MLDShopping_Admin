using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MLDShopping_Admin.Services;

namespace MLDShopping_Admin.Controllers
{
    public class PrototypeController : Controller
    {
        private readonly IAzureBlobService _azureBlobService;

        public PrototypeController(IAzureBlobService azureBlobService)
        {
            _azureBlobService = azureBlobService;
        }
        public IActionResult Index()
        {
            //test list of images
            try
            {
                var allBlobs = _azureBlobService.ListAsync("assets");
                return View(allBlobs);
            }
            catch (Exception e) 
            {

                throw;
            }
            return View();
        }
    }
}