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
using MLDShopping_Admin.Models;
using MLDShopping_Admin.Services;
using Newtonsoft.Json;

namespace MLDShopping_Admin.Areas.Products.Controllers
{
    [Area("Products")]
    public class ProductsController : Controller
    {
        private readonly CMSShoppingContext _db;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IAzureBlobService _azureBlobService;
        public ProductsController(CMSShoppingContext db, IMapper mapper, IConfiguration configuration, IAzureBlobService azureBlobService)
        {
            _db = db;
            _mapper = mapper;
            _configuration = configuration;
            _azureBlobService = azureBlobService;
        }
        public IActionResult Index()
        {
            ViewBag.PageHeader = "Products";
            //ViewBag.Description = "Do account stuff here";
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
            //if (String.IsNullOrEmpty(searchBy))
            //{
            //    sortBy = "AccountId";
            //    sortDir = true;
            //}
            var res = _db.Products
                  .Where(a => a.DeletedAt == null)
                  .Where(w => w.Name.Contains(searchBy) || w.Description1.Contains(searchBy) || w.Description2.Contains(searchBy) || w.ProductId.ToString().Contains(searchBy))
                  .Select(p => new ProductVM
                  {
                      ProductId = p.ProductId,
                      Name = p.Name,
                      Description1 = p.Description1,
                      Description2 = p.Description2,
                      IsActive = p.IsActive,
                      AddedAt = p.AddedAt.Value.ToShortDateString(),
                      Category = new CategoryVM { CategoryId = p.CategoryId, Name = p.Name },
                      MediaUrls = p.MediaUrls.Select(s => s.MediaPath).ToList()
                  })
                  .OrderBy(sortBy + " " + sortDir); // have to give a default order when skipping .. so use the PK
            var result = res
                .Skip(skip)
                .Take(take)
                .ToList();
            //.ToList();
            totalResultsCount = _db.Products.Count();
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
            var model = new ProductVM();
            var entityInDb = _db.Products.Find(id);
            if (entityInDb != null)
            {
                model = _mapper.Map<ProductVM>(entityInDb);
                model.ProductId = entityInDb.ProductId;
                model.Category = new CategoryVM { CategoryId = entityInDb.CategoryId, Name = entityInDb.Category.Name };
                //model.SubCategory = new SubCategoryVM { SubCategoryId = entityInDb.SubCategoryId.Value, Name = entityInDb.SubCategory.Name };
                model.MediaUrls = entityInDb.MediaUrls.Select(s => s.MediaPath).ToList();
                // get existing file 
                if (entityInDb.MediaUrls != null)
                {
                    foreach (var mediaUrl in entityInDb.MediaUrls)
                    {
                        var filePath = await _azureBlobService.GetUriByNameAsync(mediaUrl.MediaPath, "assets");
                        model.MediaUrls.Add(filePath.AbsoluteUri);

                    }
                }
            }
            // set values to be put into the _Layout page
            if (id == 0)
            {
                ViewData["PageHeader"] = "Product Create";
            }
            else
            {
                ViewData["PageHeader"] = "Product Edit";
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
        public async Task<IActionResult> Edit(ProductVM product, IFormFile ImageFile)
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
                var entity = _db.Products.Find(product.ProductId);
                if (entity == null)
                {
                    entity = _mapper.Map<Product>(product);
                    _db.Add(entity);
                    _db.SaveChanges();
                    //update file url
                    foreach (var url in product.MediaFiles)
                    {
                        var fileName = await _azureBlobService.UploadSingleAsync(url, "products");
                        var mediaUrlModel = new MediaUrl();
                        mediaUrlModel.ProductId = entity.ProductId;
                        mediaUrlModel.MediaPath = fileName;
                        mediaUrlModel.IsImage = true; // we will assume all files uploaded will be images
                        _db.MediaUrls.Add(mediaUrlModel);
                    }
                    _db.SaveChanges();

                    message.Text = "Product has been created.";
                    message.MessageType = MessageType.Success;

                }
                else
                {
                    entity = _mapper.Map<Product>(product);
                    entity.ProductId = product.ProductId;

                    ////update all files if an image is r 
                    //foreach (var url in product.MediaFiles)
                    //{
                    //    var fileName = url.FileName;
                    //    var productHasFileName = _db.Products.FirstOrDefault(p=> p.ProductId == product.ProductId).Produ
                    //}
                    //var fileName = await _azureBlobService.UploadSingleAsync(account.UserImage, "assets");
                    //entity.UserImageUrl = fileName;
                    ////entity = _mapper.Map<Account>(account);

                    _db.Update(entity);
                    _db.SaveChanges();
                    message.Text = "Account has been updated.";
                    message.MessageType = MessageType.Success;
                }

                _db.SaveChanges();
            }
            return RedirectToAction("Edit", new { id = product.ProductId, message = message.Text, messageType = message.MessageType.ToString() });
        }
        //public IActionResult Delete(int id)
        //{
        //    var message = new Message();

        //    var accountToDelete = _db.Accounts.Include(a => a.AccountPermissions).FirstOrDefault(a => a.AccountId == id);

        //    try
        //    {
        //        _db.AccountPermissions.RemoveRange(accountToDelete.AccountPermissions);
        //        _db.Accounts.Remove(accountToDelete);
        //        _db.SaveChanges();
        //        message.Text = "Account has been deleted";
        //        message.MessageType = MessageType.Success;

        //    }
        //    catch (Exception e)
        //    {
        //        message.Text = "An error has occurred.";
        //        message.MessageType = MessageType.Danger;
        //    }
        //    return RedirectToAction("Edit", new { id = 0, message = message.Text, messageType = message.MessageType });
        //}
    }
}