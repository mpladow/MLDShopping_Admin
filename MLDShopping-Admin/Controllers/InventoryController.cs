using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MLDShopping_Admin.Entities;
using MLDShopping_Admin.Models;
using Newtonsoft.Json;

namespace MLDShopping_Admin.Controllers
{
    public class InventoryController : Controller
    {
        private readonly CMSShoppingContext _db;
        private readonly IMapper _mapper;
        public InventoryController(CMSShoppingContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var model = new List<ProductVM>();
            var ITEMTEST2 = new ProductVM() { ProductId = 2, Image = "noneYet", Price = 10.0, Quantity = 2, Category = "Test", Name = "Test Item", Description = "None yet" };
            var ITEMTEST3 = new ProductVM() { ProductId = 3, Image = "noneYet", Price = 10.0, Quantity = 2, Category = "Test", Name = "Test Item", Description = "None yet" };

            for (int i = 0; i < 15; i++)
            {
                var ITEMTEST = new ProductVM() { ProductId = i, Image = "noneYet", Price = 10.0, Quantity = 2, Category = "Test", Name = "Test Item", Description = "None yet" };
                model.Add(ITEMTEST);

            }

            ViewData["Title"] = "Inventory";
            return View(model);
        }
        public string Read()
        {
            var model = new List<ProductVM>();
            var ITEMTEST2 = new ProductVM() { ProductId = 2, Image = "noneYet", Price = 10.0, Quantity = 2, Category = "Test", Name = "Test Item", Description = "None yet" };
            var ITEMTEST3 = new ProductVM() { ProductId = 3, Image = "noneYet", Price = 10.0, Quantity = 2, Category = "Test", Name = "Test Item", Description = "None yet" };

            for (int i = 0; i < 15; i++)
            {
                var ITEMTEST = new ProductVM() { ProductId = i, Image = "noneYet", Price = 10.0, Quantity = 2, Category = "Test", Name = "Test Item", Description = "None yet" };
                model.Add(ITEMTEST);

            }
            var json = JsonConvert.SerializeObject(model);
            var xx = Json(model);
            return json;
        }
        public IActionResult Edit(int id = 0, string message = "")
        {
            // get item from database
            // if 0 or not found, return empty model
            var model = new ProductVM();


            return View(model);
        }
    }
}