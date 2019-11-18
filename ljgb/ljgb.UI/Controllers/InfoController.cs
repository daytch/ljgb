using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ljgb.UI.Controllers
{
    public class InfoController : BaseController
    {
        private readonly ApplicationSettings _settings;

        public InfoController(IOptions<ApplicationSettings> settings)
        {
            _settings = settings.Value;
        }

        public IActionResult AppLicensing() => View(_settings);
        public IActionResult AppFlavors() => View(_settings);
    }
}
