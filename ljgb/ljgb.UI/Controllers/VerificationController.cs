using ljgb.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ljgb.UI.Controllers
{
    public class VerificationController : Controller
    {
        private readonly ApplicationSettings _settings;
        private readonly ConfigOptions _config;

        public VerificationController(IOptions<ApplicationSettings> settings, IOptions<ConfigOptions> config)
        {
            _settings = settings.Value;
            _config = config.Value;
        }
        // GET
        public IActionResult Salesman()
        {
            ViewBag.url_api = _config.base_api_url;
            return View(_settings);
        }
        public IActionResult Customer()
        {
            ViewBag.url_api = _config.base_api_url;
            return View(_settings);
        }
    }
}