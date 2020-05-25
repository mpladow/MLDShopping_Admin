using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MLDShopping_Admin.Entities;

namespace MLDShopping_Admin.Areas.Products.Data
{
    public class AjaxController : Controller
    {
        private CMSShoppingContext _db;
        public AjaxController(CMSShoppingContext db)
        {
            _db = db;
        }
        public List<SelectListItem> GetCategoriesList()
        {
            return _db.Categories.Select(p => new SelectListItem()
            {
                Value = p.CategoryId.ToString(),
                Text = p.Name
            }).ToList();
        }
        //public List<SelectListItem> GetSubCategoryList()
        //{
        //    return _db.SubCategories.Select(p => new SelectListItem()
        //    {
        //        Value = p.SubCategoryId.ToString(),
        //        Text = p.Name
        //    }).ToList();
        //}
    }
}