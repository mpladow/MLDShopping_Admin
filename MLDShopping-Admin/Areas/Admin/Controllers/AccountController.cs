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

namespace MLDShopping_Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    [BreadcrumbActionFilter]
    public class AccountController : Controller
    {
        private readonly CMSShoppingContext _db;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IAzureBlobService _azureBlobService;
        public AccountController(CMSShoppingContext db, IMapper mapper, IConfiguration configuration, IAzureBlobService azureBlobService)
        {
            _db = db;
            _mapper = mapper;
            _configuration = configuration;
            _azureBlobService = azureBlobService;
        }
        public IActionResult Index()
        {
            ViewBag.PageHeader = "Account";
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
            var res = _db.Accounts
                  .Where(a => a.DeletedAt == null)
                  .Where(w => w.Email.Contains(searchBy) || w.FirstName.Contains(searchBy) || w.LastName.Contains(searchBy) || w.AccountId.ToString().Contains(searchBy))
                  .Select(s => new AccountVM
                  {
                      AccountId = s.AccountId,
                      FirstName = s.FirstName,
                      LastName = s.LastName,
                      CreatedAt = s.CreatedAt.ToShortDateString(),
                      Deleted = s.DeletedAt.HasValue,
                      PermissionsString = String.Join(",", s.AccountPermissions
                       .Select(p => p.Permission.Name).ToList())
                  })
                  .OrderBy(sortBy + " " + sortDir); // have to give a default order when skipping .. so use the PK
            var result = res
                .Skip(skip)
                .Take(take)
                .ToList();
            //.ToList();
            totalResultsCount = _db.Accounts.Count();
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
            var model = new AccountVM();
            var entityInDb = _db.Accounts.Find(id);
            if (entityInDb != null)
            {
                model = _mapper.Map<AccountVM>(entityInDb);
                model.AccountId = entityInDb.AccountId;
                model.FirstName = entityInDb.FirstName;
                model.LastName = entityInDb.LastName;
                model.Email = entityInDb.Email;
                model.CreatedAt = entityInDb.CreatedAt.ToShortDateString();
                // get existing file 
                if (entityInDb.UserImageUrl != null)
                {
                    var filePath = await _azureBlobService.GetUriByNameAsync(entityInDb.UserImageUrl, "assets");
                    model.UserImageUrl = filePath.AbsoluteUri;
                }
                var accountPermissions = _db.AccountPermissions.Where(ap => ap.AccountId == entityInDb.AccountId).ToList();

                if (accountPermissions.Count > 0)
                {
                    model.PermissionIds = accountPermissions.Select(ap => ap.PermissionId.ToString()).ToList();
                }
            }
            // set values to be put into the _Layout page
            if (id == 0)
            {
                ViewData["PageHeader"] = "Account Create";
            }
            else
            {
                ViewData["PageHeader"] = "Account Edit";
            }
            // SET values for message
            if (message != null)
            {
                ViewData["Message"] = message;
                ViewData["MessageType"] = messageType.ToLower();
            }
            model.SelectList = this.GenerateSelectLists();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(AccountVM account, IFormFile ImageFile)
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
                var x = _db.Accounts.Include(a => a.AccountPermissions).FirstOrDefault(a => a.AccountId == account.AccountId);
                var entity = _db.Accounts.Find(account.AccountId);
                var hasher = new PasswordHasher();
                if (entity == null)
                {
                    entity = _mapper.Map<Account>(account);
                    //update file url
                    var fileName = await _azureBlobService.UploadSingleAsync(account.UserImage, "assets");
                    entity.UserImageUrl = fileName;
                    if (!string.IsNullOrEmpty(account.Password))
                    {
                        var hashedPassword = hasher.HashPassword(account.Password);
                        entity.Password = hashedPassword;
                    }
                    entity.CreatedAt = DateTime.Now;
                    _db.Add(entity);
                    _db.SaveChanges();
                    account.AccountId = entity.AccountId;
                    account.PermissionIds.ForEach(p =>
                    {
                        var permissionId = int.Parse(p);
                        var accountPermissionEntity = new AccountPermission();
                        accountPermissionEntity.PermissionId = permissionId;
                        accountPermissionEntity.AccountId = entity.AccountId;
                        _db.AccountPermissions.Add(accountPermissionEntity);
                    });



                    message.Text = "Account has been created.";
                    message.MessageType = MessageType.Success;

                }
                else
                {
                    _db.AccountPermissions.RemoveRange(entity.AccountPermissions);
                    entity.FirstName = account.FirstName;
                    entity.LastName = account.LastName;
                    entity.Email = account.Email;
                    //update file url
                    var fileName = await _azureBlobService.UploadSingleAsync(account.UserImage, "assets");
                    entity.UserImageUrl = fileName;
                    //entity = _mapper.Map<Account>(account);

                    if (!string.IsNullOrEmpty(account.Password))
                    {
                        var hashedPassword = hasher.HashPassword(account.Password);
                        entity.Password = hashedPassword;
                        var verifyPassword = hasher.VerifyHashedPassword(hashedPassword, account.Password);
                    }

                    account.PermissionIds.ForEach(p =>
                    {
                        var permissionId = int.Parse(p);
                        var accountPermissionEntity = new AccountPermission();
                        accountPermissionEntity.PermissionId = permissionId;
                        accountPermissionEntity.AccountId = entity.AccountId;
                        _db.AccountPermissions.Add(accountPermissionEntity);
                        _db.SaveChanges();

                    });
                    _db.Update(entity);
                    _db.SaveChanges();
                    message.Text = "Account has been updated.";
                    message.MessageType = MessageType.Success;
                }

                _db.SaveChanges();
            }
            account.SelectList = GenerateSelectLists();
            return RedirectToAction("Edit", new { id = account.AccountId, message = message.Text, messageType = message.MessageType.ToString() });
        }
        public IActionResult Delete(int id)
        {
            var message = new Message();

            var accountToDelete = _db.Accounts.Include(a => a.AccountPermissions).FirstOrDefault(a => a.AccountId == id);

            try
            {
                _db.AccountPermissions.RemoveRange(accountToDelete.AccountPermissions);
                _db.Accounts.Remove(accountToDelete);
                _db.SaveChanges();
                message.Text = "Account has been deleted";
                message.MessageType = MessageType.Success;

            }
            catch (Exception e)
            {
                message.Text = "An error has occurred.";
                message.MessageType = MessageType.Danger;
            }
            return RedirectToAction("Edit", new { id = 0, message = message.Text, messageType = message.MessageType });
        }
        public List<SelectListItem> GenerateSelectLists()
        {
            return _db.Permissions.Select(p => new SelectListItem()
            {
                Value = p.PermissionId.ToString(),
                Text = p.Name
            }).ToList();
        }
    }
}