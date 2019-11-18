using ljgb.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ljgb.UI.Controllers
{
    public class KotaController : BaseController
    {
        private readonly ApplicationSettings _settings;
        public string returnUrl { get; set; }
        private static string base_url_api;
        public KotaController(IOptions<ApplicationSettings> settings, ConfigOptions _urlapi)
        {
            _settings = settings.Value;
            base_url_api = _urlapi.base_api_url;
        }
        public IActionResult Kota()
        {

            ViewBag.url_api = base_url_api;

            return View(_settings);
          
        }
    }
}