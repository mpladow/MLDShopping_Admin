using MLDShopping_Admin.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MLDShopping_Admin.Entities.Models;

namespace MLDShopping_Admin.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<Account, AccountVM>();
            CreateMap<AccountVM, Account>();
            CreateMap<Category, CategoryVM>();
            CreateMap<CategoryVM, Category>();
            CreateMap<Product, ProductVM>();
            CreateMap<ProductVM, Product>();
        }
    }
}
