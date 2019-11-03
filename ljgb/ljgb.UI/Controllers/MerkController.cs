using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ljgb.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ljgb.UI.Controllers
{
    public class MerkController : Controller
    {
        private readonly ApplicationSettings _settings;
        private readonly ConfigOptions _config;

        public MerkController(IOptions<ApplicationSettings> settings, IOptions<ConfigOptions> config)
        {
            _settings = settings.Value;
            _config = config.Value;
        }
        public IActionResult ListMerk()
        {
            ViewBag.url_api = _config.base_api_url;
            return View(_settings);
        }
    }
}