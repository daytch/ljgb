using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using ljgb.Common.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Flurl.Http;

namespace ljgb.UI.Areas.Identity.Pages.Account
{
    public class ActivationAccountModel : PageModel
    {
        private static string api_url = string.Empty;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;

        public ActivationAccountModel(SignInManager<IdentityUser> signInManager, ILogger<LogoutModel> logger, UserManager<IdentityUser> userManager, IEmailSender emailSender)
        {
            _signInManager = signInManager;
            _logger = logger;
            _userManager = userManager;
            _emailSender = emailSender;

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            api_url = configuration.GetSection("API_url").Value;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
            public string ErrorMessage { get; set; }
        }

        public async Task OnGet()
        {
            await _signInManager.SignOutAsync();

            _logger.LogInformation("User logged out.");
        }

        public async Task<IActionResult> OnPostAsync()
        {
       
                string url = api_url + "Auth/ActivateAccount";
                string token = HttpContext.Request.Query["token"];
                UserResponse response = await url.PostJsonAsync(new
                {
                    Token = token
                }).ReceiveJson<UserResponse>();

                if (response.IsSuccess)
                {
                    return RedirectToPage("./ActivationAccountConfirmation");
                }
                else
                {
                    Input.ErrorMessage = response.Message;
                    return Page();
                }
            

            return Page();
        }
    }
}