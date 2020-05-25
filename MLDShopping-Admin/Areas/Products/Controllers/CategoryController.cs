using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MLDShopping_Admin.Components;
using MLDShopping_Admin.Entities;
using MLDShopping_Admin.Entities.Models;
using MLDShopping_Admin.Models;
using MLDShopping_Admin.Services;
using Newtonsoft.Json;

namespace MLDShopping_Admin.Areas.Products.Controllers
{
    [Area("Products")]
    public class CategoryController : Controller
    {
        private CMSShoppingContext _db;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IAzureBlobService _azureBlobService;

        public CategoryController(CMSShoppingContext db, IMapper mapper, IConfiguration configuration, IAzureBlobService azureBlobService)
        {
            _db = db;
            _mapper = mapper;
            _configuration = configuration;
            _azureBlobService = azureBlobService;
        }
        public IActionResult Index()
        {
            ViewBag.PageHeader = "Category";
            ViewBag.Description = "Create new categories for your products.";

            return View();
        }

        public JsonResult Read([FromBody]DataTableAjaxPostModel model)
        {
            // action inside a standard controller
            int totalResultsCount;
            int filteredResultsCount;

            //data from model
            var searchBy = (model.search != null) ? model.search.value : null;
            var take = model.length;
            var skip = model.start;

            string sortBy = "";
            string sortDir = "";

            if (model.order != null)
            {
                // in this example we just default sort on the 1st column
                sortBy = model.columns[model.order[0].column].data;
                sortDir = model.order[0].dir.ToLower();
            }

            var res = _db.Categories
                 .Where(a => a.DeletedAt == null)
                  .Where(w => w.Name.Contains(searchBy) || w.Description.Contains(searchBy) || w.CategoryId.ToString().Contains(searchBy))
                  .Select(s => new CategoryVM
                  {
                      CategoryId = s.CategoryId,
                      Name = s.Name,
                      Description = s.Description
                  })
                  .OrderBy(sortBy + " " + sortDir); // have to give a default order when skipping .. so use the PK
            var result = res
                .Skip(skip)
                .Take(take)
                .ToList();
            //.ToList();
            totalResultsCount = _db.Categories.Count();
            filteredResultsCount = res.Count();

            var json = JsonConvert.SerializeObject(model);
            return Json(new
            {
                // this is what datatables whats sent back
                draw = model.draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = result
            });
        }
        public async Task<IActionResult> Edit(int id, string message = "", string messageType = "")
        {
            var model = new CategoryVM();
            var entityInDb = _db.Categories.Find(id);
            if (entityInDb != null)
            {
                model = _mapper.Map<CategoryVM>(entityInDb);
                model.CategoryId = entityInDb.CategoryId;
                model.Name = entityInDb.Name;
                model.Description = entityInDb.Description;
            }
            // set values to be put into the _Layout page
            if (id == 0)
            {
                ViewData["PageHeader"] = "Category Create";
            }
            else
            {
                ViewData["PageHeader"] = "Category Edit";
            }
            // SET values for message
            if (message != null)
            {
                ViewData["Message"] = message;
                ViewData["MessageType"] = messageType.ToLower();
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(CategoryVM category, IFormFile ImageFile)
        {
            var message = new Message();


            if (!ModelState.IsValid)
            {
                message.Text = "Please check your form details";
                message.MessageType = MessageType.Warning;
                return BadRequest();
            }
            else
            {
                var entity = _db.Categories.Find(category.CategoryId);
                var hasher = new PasswordHasher();
                if (entity == null)
                {
                    entity = _mapper.Map<Category>(category);
                    //update file url
                    _db.Add(entity);
                    _db.SaveChanges();

                    message.Text = "Category has been created.";
                    message.MessageType = MessageType.Success;

                }
                else
                {
                    entity.Name = category.Name;
                    entity.Description = category.Description;
                    _db.Update(entity);
                    _db.SaveChanges();
                    message.Text = "Category has been updated.";
                    message.MessageType = MessageType.Success;
                }

                _db.SaveChanges();
            }
            return RedirectToAction("Edit", new { id = category.CategoryId, message = message.Text, messageType = message.MessageType.ToString() });
        }

    }
}