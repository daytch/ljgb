using System.Threading.Tasks;
using Flurl.Http;
using ljgb.Common.Requests;
using ljgb.Common.Responses;
using ljgb.UI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using static ljgb.UI.Areas.Identity.Pages.Account.ExternalLoginModel;

namespace ljgb.UI.Controllers
{
    public class ProvinsiController : Controller
    {
        private readonly ApplicationSettings _settings;
        //private readonly ILogger<RegisterModel> _logger;
        //private readonly IEmailSender _emailSender;
        //private readonly SignInManager<IdentityUser> _signInManager;
        public string returnUrl { get; set; }
        private readonly UserManager<IdentityUser> _userManager;
        [BindProperty] public InputModel Input { get; set; }
        private static string base_url_api;
        public ProvinsiController(IOptions<ApplicationSettings> settings, ConfigOptions _urlapi)
        {
            _settings = settings.Value;
            base_url_api = _urlapi.base_api_url;
        }
        public async Task<IActionResult> Provinsi()
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ProvinsiRequest request = new ProvinsiRequest();
            if (ModelState.IsValid)
            {
                string url_api = base_url_api + "Provinsi/GetAll";
                ViewBag.url_api = base_url_api;


                try
                {
                    var result = await url_api.PostJsonAsync(request).ReceiveJson<ProvinsiResponse>();
                    ViewData["Response"] = result;
                }
                catch (FlurlHttpTimeoutException ext)
                {
                    // FlurlHttpTimeoutException derives from FlurlHttpException; catch here only
                    // if you want to handle timeouts as a special case
                    // Console.log("Request timed out.");
                    var b = ext;
                }
                catch (FlurlHttpException ex)
                {
                    // ex.Message contains rich details, inclulding the URL, verb, response status,
                    // and request and response bodies (if available)
                    var a = ex;//(ex.Message);
                }
            }
             
            return View(_settings);
        }

        public async Task<IActionResult> AddProvinsi([FromBody] ProvinsiRequest request)
        {

            return View(_settings);

        }

        private async void Load()
        {
            //returnUrl = returnUrl ?? Url.Content("~/");
            //if (ModelState.IsValid)
            //{
            //    var user = new IdentityUser { UserName = Input.Email, Email = Input.Email };
            //    //IdentityResult result = await _userManager.CreateAsync(user, Input.Password);
            //    //UserRequest u = new UserRequest() { user = user};
            //    string url = base_url_api + "User/GetAll";
            //    IdentityResult result = new IdentityResult();
            //    try
            //    {
            //        result = await url.PostJsonAsync(u).ReceiveJson<ProvinsiResponse>();
            //    }
            //    catch (FlurlHttpTimeoutException ext)
            //    {
            //        // FlurlHttpTimeoutException derives from FlurlHttpException; catch here only
            //        // if you want to handle timeouts as a special case
            //        // Console.log("Request timed out.");
            //        var b = ext;
            //    }
            //    catch (FlurlHttpException ex)
            //    {
            //        // ex.Message contains rich details, inclulding the URL, verb, response status,
            //        // and request and response bodies (if available)
            //        var a = ex;//(ex.Message);
            //    }

            //    if (result.Succeeded)
            //    {

            //    }

            //    foreach (var error in result.Errors)
            //    {
            //        ModelState.AddModelError(string.Empty, error.Description);
            //    }
            //}
        }

    }
}