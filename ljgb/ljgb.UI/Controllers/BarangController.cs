using ljgb.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ljgb.UI.Controllers
{
    //[Authorize]
    public class BarangController : BaseController
    {
        private readonly ApplicationSettings _settings;
        private readonly ConfigOptions _config;
        public BarangController(IOptions<ApplicationSettings> settings, IOptions<ConfigOptions> config)
        {
            _settings = settings.Value;
            _config = config.Value;
        }

        public IActionResult ListBarang()
        {
            ViewBag.url_api = _config.base_api_url;
            return View(_settings);
        }

        public IActionResult Upload()
        {
            ViewBag.url_api = _config.base_api_url;
            return View(_settings);
        }

        public IActionResult HighestBids()
        {
            ViewBag.url_api = _config.base_api_url;
            return View(_settings);
        }
        public IActionResult LowestAsks()
        {
            ViewBag.url_api = _config.base_api_url;
            return View(_settings);
        }
        public IActionResult OtherCategory()
        {
            ViewBag.url_api = _config.base_api_url;
            return View(_settings);
        }

    }
}
