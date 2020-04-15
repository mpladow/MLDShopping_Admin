﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MLDShopping_Admin.Models;

namespace MLDShopping_Admin.Controllers
{
    public class InventoryController : Controller
    {
        public IActionResult Index()
        {
            var model = new List<ProductVM>();
            var ITEMTEST = new ProductVM() { ProductId = 1, Image = "noneYet", Price = 10.0, Quantity = 2, Category = "Test", Name = "Test Item", Description = "None yet" };
            var ITEMTEST2 = new ProductVM() { ProductId = 2, Image = "noneYet", Price = 10.0, Quantity = 2, Category = "Test", Name = "Test Item", Description = "None yet" };
            var ITEMTEST3 = new ProductVM() { ProductId = 3, Image = "noneYet", Price = 10.0, Quantity = 2, Category = "Test", Name = "Test Item", Description = "None yet" };

            model.Add(ITEMTEST);
            model.Add(ITEMTEST2);
            model.Add(ITEMTEST3);

            ViewData["Title"] = "Inventory";
            return View(model);
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