using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MLDShopping_Admin.Services
{
    public interface IUserDetailsService
    {
        string GetEmail();
        string GetFirstName();
        string GetLastName();
        string GetUserImageUrl();
    }
    public class UserDetailsService : IUserDetailsService {

        private IHttpContextAccessor _httpContextAccessor;
        private readonly IAzureBlobService _azureBlobService;

        public UserDetailsService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string GetFirstName()
        {
            var firstName = ((ClaimsIdentity)_httpContextAccessor.HttpContext.User.Identity).Claims.FirstOrDefault(c => c.Type == "FirstName").Value;
            if (!string.IsNullOrEmpty(firstName))
                return firstName;
            else
                return String.Empty;

        }
        public string GetLastName()
        {
            var lastName = ((ClaimsIdentity)_httpContextAccessor.HttpContext.User.Identity).Claims.FirstOrDefault(c => c.Type == "LastName").Value;
            if (!string.IsNullOrEmpty(lastName))
                return lastName;
            else
                return String.Empty;
        }
        public string GetEmail()
        {
            var email = ((ClaimsIdentity)_httpContextAccessor.HttpContext.User.Identity).Claims.FirstOrDefault(c => c.Type == "Email").Value;

            if (!string.IsNullOrEmpty(email))
                return email;
            else
                return string.Empty;
        }
        public string GetUserImageUrl()
        {
            var url = _httpContextAccessor.HttpContext.Request.Cookies["UserImage"];
            if (!string.IsNullOrEmpty(url))
                return url;
            else
                return String.Empty;
        }
    }
}
