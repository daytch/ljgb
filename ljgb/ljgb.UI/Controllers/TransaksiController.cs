using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl.Http;
using ljgb.Common.Requests;
using ljgb.Common.Responses;
using ljgb.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ljgb.UI.Controllers
{
    public class TransaksiController : BaseController
    {
        private readonly ApplicationSettings _settings;
        public string returnUrl { get; set; }
        private static string base_url_api;

        public TransaksiController(IOptions<ApplicationSettings> settings, ConfigOptions _urlapi)
        {
            _settings = settings.Value;
            base_url_api = _urlapi.base_api_url;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Transaksi()
        {

            returnUrl = returnUrl ?? Url.Content("~/");
            TransactionRequest request = new TransactionRequest();
            if (ModelState.IsValid)
            {
                string url_api = base_url_api + "Transaction/GetAll";
                ViewBag.url_api = base_url_api;


                try
                {
                    var result = await url_api.PostJsonAsync(request).ReceiveJson<TransactionResponse>();
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

        public async Task<IActionResult> DetailProfile(long id)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                string url_api = base_url_api + "User/GetPost";
                ViewBag.url_api = base_url_api;
                

                try
                {
                    var result = await url_api.PostJsonAsync(id).ReceiveJson<UserResponse>();
                    ViewData["DetailResponse"] = result;
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
            return PartialView(2);
        }

        public IActionResult Report()
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            TransactionRequest request = new TransactionRequest();
            if (ModelState.IsValid)
            {
                string url_api = base_url_api + "Transaction/GetAll";
                ViewBag.url_api = base_url_api;
            }
            return View(_settings);
        }

    }
}