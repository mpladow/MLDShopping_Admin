using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MLDShopping_Admin.Components
{
    public static class Extensions
    {
        public static Type GetControllerType(string name)
        {
            Type controller = null;

            try
            {
                controller = Assembly.GetCallingAssembly().GetType("MLDShopping_Admin.Controllers." + name);
            }
            catch
            { }

            return controller;
        }

        public static string CamelCaseSpacing(string s)
        {
            // Sourced from https://stackoverflow.com/questions/4488969/split-a-string-by-capital-letters.
            var r = new Regex(@"
        (?<=[A-Z])(?=[A-Z][a-z]) |
         (?<=[^A-Z])(?=[A-Z]) |
         (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);

            return r.Replace(s, " ");
        }
        public static string UppercaseFirst(this string s)
        {
            // Check for empty string
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        public static string GetSpecificClaim(this ClaimsIdentity claimsIdentity, string claimType)
        {
            var claim = claimsIdentity.Claims.FirstOrDefault(x => x.Type == claimType);
            return (claim != null) ? claim.Value : string.Empty;
        }
        public static string GetFirstName(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("FirstName");
            // Test for null to avoid issues during local testing
            // return (claim != null) ? claim.Value : string.Empty;
            return claim.ToString();
        }
    }

}
