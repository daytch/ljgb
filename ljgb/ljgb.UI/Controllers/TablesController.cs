using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ljgb.UI.Controllers
{
    public class TablesController : BaseController
    {
        private readonly ApplicationSettings _settings;

        public TablesController(IOptions<ApplicationSettings> settings)
        {
            _settings = settings.Value;
        }
        // GET
        public IActionResult Basic() => View(_settings);
        public IActionResult GenerateStyle() => View(_settings);
        
    }
}
