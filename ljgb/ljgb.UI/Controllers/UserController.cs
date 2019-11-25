using ljgb.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ljgb.UI.Controllers
{
    //[Authorize]
    public class UserController : BaseController
    {
        private readonly ApplicationSettings _settings;
        private readonly ConfigOptions _config;
        public UserController(IOptions<ApplicationSettings> settings, IOptions<ConfigOptions> config)
        {
            _settings = settings.Value;
            _config = config.Value;
        }

        public IActionResult UserProfile()
        {
            ViewBag.url_api = _config.base_api_url;
            return View(_settings);
        }

        public IActionResult ChangePassword()
        {
            ViewBag.url_api = _config.base_api_url;
            return View(_settings);
        }
      
    }
}
