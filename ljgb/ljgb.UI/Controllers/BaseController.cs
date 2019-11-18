using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net.Http;
using System.Web;
using System.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using ljgb.Common.Models;

namespace ljgb.UI.Controllers
{
    public class BaseController : Controller, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string access_token = string.Empty;
            filterContext.HttpContext.Request.Cookies.TryGetValue("access_token", out access_token);

            if (!string.IsNullOrEmpty(access_token))
            {
                var jwthandler = new JwtSecurityTokenHandler();
                var jwttoken = jwthandler.ReadToken(access_token);
                var expDate = jwttoken.ValidTo;
                if (expDate < DateTime.UtcNow.AddMinutes(1))
                    access_token = "";// GetAccessToken().Result;
                else
                    filterContext.HttpContext.Response.Cookies.Append("access_token", access_token);
            }
            else
            {
                Response.Redirect("/Identity/Account/Login");
            }
        }
    }
}