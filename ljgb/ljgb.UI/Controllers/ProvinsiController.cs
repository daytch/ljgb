using System.Threading.Tasks;
using Flurl.Http;
using ljgb.Common.Requests;
using ljgb.Common.Responses;
using ljgb.UI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using static ljgb.UI.Areas.Identity.Pages.Account.ExternalLoginModel;

namespace ljgb.UI.Controllers
{
    public class ProvinsiController : Controller
    {
        private readonly ApplicationSettings _settings;
        public string returnUrl { get; set; }
        private static string base_url_api;
        public ProvinsiController(IOptions<ApplicationSettings> settings, ConfigOptions _urlapi)
        {
            _settings = settings.Value;
            base_url_api = _urlapi.base_api_url;
        }
        public async Task<IActionResult> Provinsi()
        {
            ViewBag.url_api = base_url_api; 
            return View(_settings);
        }

      

    }
}