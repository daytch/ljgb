using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ljgb.UI.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return Redirect("~/Identity/Account/Login");
        }
    }
}