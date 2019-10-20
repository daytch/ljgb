using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ljgb.UI.Models;
using System.Threading.Tasks;
using ljgb.DataAccess.Models;
using Flurl;
using Flurl.Http;
using System.Collections.Generic;

namespace ljgb.UI.Controllers
{
    [Authorize]
    public class IntelController : Controller
    {
        private readonly ApplicationSettings _settings;

        public IntelController(IOptions<ApplicationSettings> settings)
        {
            _settings = settings.Value;
        }

        public IActionResult Introduction() => View(_settings);

        public IActionResult AspNetCore() => View(_settings);

        public async Task<IActionResult> Privacy() {

            // Flurl will use 1 HttpClient instance per host
            var warna = await "https://localhost:44325/api/Warna/GetWarna".GetJsonAsync<List<Warna>>();
            return View(_settings);
        } //=> View(_settings);

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
