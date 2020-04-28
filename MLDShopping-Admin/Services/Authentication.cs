using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MLDShopping_Admin.Entities;
using MLDShopping_Admin.Interfaces;
using MLDShopping_Admin.Models;
using MLDShopping_Admin.Services;
using Newtonsoft.Json;

namespace MLDShopping_Admin.Services
{
    public class Authentication : IAuthentication
    {
        private readonly CMSShoppingContext _db;
        private readonly IMapper _mapper;

        public Authentication(CMSShoppingContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            //_passwordHasher = passwordHasher;
        }

        public AccountVM Authenticate(string username, string password)
        {
            var accountInDb = _db.Accounts.FirstOrDefault(a => a.Email == username.ToLower());
            AccountVM accountVM = null;
            var hasher = new PasswordHasher();
            if (accountInDb != null)
            {
                // hash password.
                var correctPassword = hasher.VerifyHashedPassword(accountInDb.Password, password);
                if (correctPassword == Microsoft.AspNet.Identity.PasswordVerificationResult.Success
                    || correctPassword == Microsoft.AspNet.Identity.PasswordVerificationResult.SuccessRehashNeeded)
                {
                    accountVM = _mapper.Map<AccountVM>(accountInDb);
                    // find permissions
                    accountVM.Permissions =  _db.AccountPermissions
                        .Where(ap => ap.AccountId == accountVM.AccountId)
                        .Select(s => new PermissionVM
                        {
                            PermissionId = s.PermissionId,
                            Name = s.Permission.Name,
                            Description = s.Permission.Description
                        }).ToList();

                    var xx = 1;
                }
            }
            else
            {
                //login failed, no account exists
            }
            return accountVM;
        }

        AccountVM IAuthentication.Register(AccountVM account, string user)
        {
            throw new NotImplementedException();
        }
    }
}
