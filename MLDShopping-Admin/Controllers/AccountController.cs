using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MLDShopping_Admin.Entities;
using MLDShopping_Admin.Models;
using Newtonsoft.Json;

namespace MLDShopping_Admin.Controllers
{
    public class AccountController : Controller
    {
        private readonly CMSShoppingContext _db;
        private readonly IMapper _mapper;
        public AccountController(CMSShoppingContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public IActionResult Index()
        {

            //for (int i = 0; i < 15; i++)
            //{
            //    var ITEMTEST = new AccountVM() { AccountId = i, FirstName = "Bob" + i, LastName = "mcface" + i, Email = "dfsdf" + i + "@dfdf.com", CreatedAt = i + "d/fdf", Deleted = false, Permissions = "lots, of, permissions" };
            //    model.Add(ITEMTEST);

            //}
            return View();
        }
        public string Read()
        {
            var model = _db.Accounts
                  .Where(a => a.DeletedAt == null)
                  .Select(s => new AccountVM
                  {
                      AccountId = s.AccountId,
                      FirstName = s.FirstName,
                      LastName = s.LastName,
                      CreatedAt = s.CreatedAt.ToShortDateString(),
                      Deleted = s.DeletedAt.HasValue,
                      PermissionsString = String.Join(",", s.AccountPermissions
                       .Select(p => p.Permission.Name).ToList())
                  }).ToList();

            //for (int i = 0; i < 15; i++)
            //{
            //    var ITEMTEST = new AccountVM() { AccountId = i, FirstName = "Bob" + i, LastName = "mcface" + i, Email = "dfsdf" + i + "@dfdf.com", CreatedAt = i + "d/fdf", Deleted = false, Permissions = "lots, of, permissions" };
            //    model.Add(ITEMTEST);

            //}
            var json = JsonConvert.SerializeObject(model);
            return json;
        }
        public IActionResult Edit(int id, string message = "")
        {
            var model = new AccountVM();
            var entityInDb = _db.Accounts.Include(acc => acc.AccountPermissions).Where(acc => acc.AccountId == id).FirstOrDefault();
            if (entityInDb != null)
            {
                model = _mapper.Map<AccountVM>(entityInDb);
                model.AccountId = entityInDb.AccountId;
                model.FirstName = entityInDb.FirstName;
                model.LastName = entityInDb.LastName;
                model.Email = entityInDb.Email;
                model.CreatedAt = entityInDb.CreatedAt.ToShortDateString();
                var accountPermissions = _db.AccountPermissions.Where(ap => ap.AccountId == entityInDb.AccountId).ToList();

                if (accountPermissions.Count > 0)
                {
                    model.PermissionIds = accountPermissions.Select(ap => ap.PermissionId.ToString()).ToList();
                }
                ViewData["Message"] = message;
            }
            model.SelectList = this.GenerateSelectLists();
            return View(model);
        }
        [HttpPost]

        public IActionResult Edit(AccountVM account)
        {
            var message = "";
            if (!ModelState.IsValid)
            {
                message = "Please check your form details";
                return BadRequest();
            }
            else
            {
                var entity = _db.Accounts.Find(account.AccountId);
                if (entity == null)
                {
                    entity = _mapper.Map<Account>(account);
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
                    message = "Account has been created.";

                }
                else
                {
                    entity = _mapper.Map<Account>(account);
                    var accountPermissionsToRemove = _db.AccountPermissions.Where(ap => ap.AccountId == account.AccountId).ToList();
                    _db.AccountPermissions.RemoveRange(accountPermissionsToRemove);
                    account.PermissionIds.ForEach(p =>
                    {
                        var permissionId = int.Parse(p);
                        var accountPermissionEntity = new AccountPermission();
                        accountPermissionEntity.PermissionId = permissionId;
                        accountPermissionEntity.AccountId = entity.AccountId;
                        _db.AccountPermissions.Add(accountPermissionEntity);
                        _db.SaveChanges();

                    });
                    message = "Account has been updated.";

                }

            }
            _db.SaveChanges();
            account.SelectList = GenerateSelectLists();
            ViewData["Message"] = message;
            return View(account);
        }
        public IActionResult Delete(AccountVM account)
        {
            var xx = account;
            return View();
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