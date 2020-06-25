using CraftBackEnd.Common.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CraftBackEnd.Controllers
{
    public class BaseController : Controller
    {
        protected User CurrentUser {
            get {
                return HttpContext.Items["User"] as User;
            }
        }

        protected User StaffUser {
            get {
                return HttpContext.Items["Staff"] as User;
            }
        }
    }
}
