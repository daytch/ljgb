using ljgb.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ljgb.UI.Controllers
{
    public class NegoController : BaseController
    {
        private readonly ApplicationSettings _settings;
        private readonly ConfigOptions _config;

        public NegoController(IOptions<ApplicationSettings> settings, IOptions<ConfigOptions> config)
        {
            _settings = settings.Value;
            _config = config.Value;
        }
        public IActionResult ListASK()
        {
            ViewBag.url_api = _config.base_api_url;
            return View(_settings);
        }
        public IActionResult ListBID()
        {
            ViewBag.url_api = _config.base_api_url;
            return View(_settings);
        }
        public IActionResult ListASKS()
        {
            ViewBag.url_api = _config.base_api_url;
            return View(_settings);
        }
    }
}