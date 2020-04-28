using MLDShopping_Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MLDShopping_Admin.Interfaces
{
    public interface IAuthentication
    {
        public AccountVM Authenticate(string username, string password);
        public AccountVM Register(AccountVM account, string user);

    }
}
