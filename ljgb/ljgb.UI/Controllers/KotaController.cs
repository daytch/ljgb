using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl.Http;
using ljgb.Common.Requests;
using ljgb.Common.Responses;
using ljgb.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;

namespace ljgb.UI.Controllers
{
    public class KotaController : Controller
    {
        private readonly ApplicationSettings _settings;
        public string returnUrl { get; set; }
        private static string base_url_api;
        public KotaController(IOptions<ApplicationSettings> settings, ConfigOptions _urlapi)
        {
            _settings = settings.Value;
            base_url_api = _urlapi.base_api_url;
        }
        public async Task<IActionResult> Kota()
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            KotaRequest request = new KotaRequest();
            if (ModelState.IsValid)
            {
                string url_api = base_url_api + "Kota/GetAll";
                ViewBag.url_api = base_url_api;


                try
                {
                    var result = await url_api.PostJsonAsync(request).ReceiveJson<KotaResponse>();
                    ViewData["Response"] = result;

                 
                    //ViewBag["ProvinsiSelectList"] = result.ListProvinsi.Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.Nama }).ToList();
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
    }
}