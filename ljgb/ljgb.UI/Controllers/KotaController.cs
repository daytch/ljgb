using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl.Http;
using ljgb.Common.Requests;
using ljgb.Common.Responses;
using ljgb.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;

namespace ljgb.UI.Controllers
{
    public class KotaController : Controller
    {
        private readonly ApplicationSettings _settings;
        public string returnUrl { get; set; }
        private static string base_url_api;
        public KotaController(IOptions<ApplicationSettings> settings, ConfigOptions _urlapi)
        {
            _settings = settings.Value;
            base_url_api = _urlapi.base_api_url;
        }
        public async Task<IActionResult> Kota()
        {

            ViewBag.url_api = base_url_api;

            return View(_settings);
          
        }
    }
}