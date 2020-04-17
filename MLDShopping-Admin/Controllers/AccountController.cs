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
    public class AccountController : Controller
    {
        private readonly CMSShoppingContext _db;
        private readonly IMapper _mapper;
        public AccountController(CMSShoppingContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            //var model = _db.Accounts
            //    .Where(a => a.DeletedAt == null)
            //    .Select(s => new AccountVM
            //    {
            //        AccountId = s.AccountId,
            //        FirstName = s.FirstName,
            //        LastName = s.LastName,
            //        CreatedAt = s.CreatedAt.ToShortDateString(),
            //        Deleted = s.DeletedAt.HasValue,
            //        Permissions = s.Permissions
            //         .Select(p => p.Name).FirstOrDefault()
            //    }).ToList();
            var model = new List<AccountVM>();

            for (int i = 0; i < 15; i++)
            {
                var ITEMTEST = new AccountVM() { AccountId = i, FirstName = "Bob" + i, LastName = "mcface" + i, Email = "dfsdf" + i + "@dfdf.com", CreatedAt = i + "d/fdf", Deleted = false, Permissions = "lots, of, permissions" };
                model.Add(ITEMTEST);

            }
            return View(model);
        }
        public string Read()
        {
            //var model = _db.Accounts
            //    .Select(s => new AccountVM
            //    {
            //        AccountId = s.AccountId,
            //        FirstName = s.FirstName,
            //        LastName = s.LastName,
            //        CreatedAt = s.CreatedAt.ToShortDateString(),
            //        Deleted = s.DeletedAt.HasValue,
            //        Permissions = s.Permissions
            //         .Select(p => p.Name).FirstOrDefault()
            //    }).ToList();
            var model = new List<AccountVM>();

            for (int i = 0; i < 15; i++)
            {
                var ITEMTEST = new AccountVM() { AccountId = i, FirstName = "Bob" + i, LastName = "mcface" + i, Email = "dfsdf" + i + "@dfdf.com", CreatedAt = i + "d/fdf", Deleted = false, Permissions = "lots, of, permissions" };
                model.Add(ITEMTEST);

            }
            var json = JsonConvert.SerializeObject(model);
            return json;
        }
        public IActionResult Edit(int id)
        {
            var model = new AccountVM();
            var entityInDb = _db.Accounts.Find(id);
            if (entityInDb != null)
            {
                model.AccountId = entityInDb.AccountId;
                model.FirstName = entityInDb.FirstName;
                model.LastName = entityInDb.LastName;
                model.Email = entityInDb.Email;
                model.CreatedAt = entityInDb.CreatedAt.ToShortDateString();
                model.Permissions = entityInDb.Permissions.ToString();
            }
            return View(entityInDb);
        }
        [HttpPost]
        public IActionResult Edit(AccountVM account)
        {
            return View();
        }
    }
}