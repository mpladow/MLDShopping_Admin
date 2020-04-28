using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MLDShopping_Admin.Components
{
    public class HasPermissionAttribute: AuthorizeAttribute
    {
    }
}
