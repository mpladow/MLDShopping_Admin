using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MLDShopping_Admin.Entities;
using MLDShopping_Admin.Models;
using MLDShopping_Admin.Services;
using Newtonsoft.Json;

namespace MLDShopping_Admin.Controllers
{
    public class AccountController : Controller
    {
        private readonly CMSShoppingContext _db;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;
        public AccountController(CMSShoppingContext db, IMapper mapper, IPasswordHasher passwordHasher)
        {
            _db = db;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
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
            var entityInDb = _db.Accounts.Find(id);
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
            }
            ViewData["Message"] = message;
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
                var x = _db.Accounts.Include(a => a.AccountPermissions).FirstOrDefault(a => a.AccountId == account.AccountId);
                var entity = _db.Accounts.Find(account.AccountId);
                if (entity == null)
                {
                    entity = _mapper.Map<Account>(account);
                    if (!string.IsNullOrEmpty(account.Password))
                    {
                        var hashedPassword = _passwordHasher.HashPassword(account.Password);
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
                    message = "Account has been created.";

                }
                else
                {
                    _db.AccountPermissions.RemoveRange(entity.AccountPermissions);
                    entity.FirstName = account.FirstName;
                    entity.LastName = account.LastName;
                    entity.Email = account.Email;
                    //entity = _mapper.Map<Account>(account);

                    if (!string.IsNullOrEmpty(account.Password))
                    {
                        var hashedPassword = _passwordHasher.HashPassword(account.Password);
                        entity.Password = hashedPassword;
                        var verifyPassword = _passwordHasher.VerifyHashedPassword(hashedPassword, account.Password);
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
                    message = "Account has been updated.";

                }

                _db.SaveChanges();
            }
            account.SelectList = GenerateSelectLists();
            ViewData["Message"] = message;
            return RedirectToAction("Edit", new { id=account.AccountId,  message = message });
        }
        public IActionResult Delete(int id)
        {
            var accountToDelete = _db.Accounts.Include(a => a.AccountPermissions).FirstOrDefault(a => a.AccountId == id);

            var message = "";
            try
            {
                _db.AccountPermissions.RemoveRange(accountToDelete.AccountPermissions);
                _db.Accounts.Remove(accountToDelete);
                _db.SaveChanges();
                message = "Account has been deleted";

            }
            catch (Exception e)
            {
                message = "An error has occurred.";

            }
            return RedirectToAction("Edit", new { message = message });
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