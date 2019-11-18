using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ljgb.UI.Data;
using ljgb.UI.Models;
using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Logging;
using System;

namespace ljgb.UI
{
    public class Startup
    {
        private readonly ILogger<Startup> logger;

        public Startup(IConfiguration configuration, ILogger<Startup> _log)
        {
            Configuration = configuration;
            logger = _log;
        }// => Configuration = configuration;

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ApplicationSettings>(Configuration.GetSection(ApplicationSettings.SectionKey));
            services.AddSingleton(s => s.GetRequiredService<IOptions<ApplicationSettings>>().Value);
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // Note: The default connection string assumes that you have 'LocalDb' installed on your machine (either through SQL Server or Visual Studio installer)
            // If you followed the instructions in 'README.MD' and installed SQL Express then change the 'DefaultConnection' value in 'appSettings.json' with
            // "Server=localhost\\SQLEXPRESS;Database=aspnet-smartadmin;Trusted_Connection=True;MultipleActiveResultSets=true"
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            services.AddRouting(options =>
                {
                    options.LowercaseUrls = true;
                    options.LowercaseQueryStrings = true;
                })
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddRazorPagesOptions(options =>
                {
                    options.AllowAreas = true;
                    options.Conventions.AuthorizeAreaFolder("Identity", "/Account/Manage");
                    options.Conventions.AuthorizeAreaPage("Identity", "/Account/Logout");
                });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Identity/Account/Login";
                options.LogoutPath = "/Identity/Account/Logout";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            });

            // Do this in Startup. All calls to SimpleCast will use the same HttpClient instance.
            FlurlHttp.ConfigureClient(Configuration["API_url"], cli => cli
                .Configure(settings =>
                {
                    // keeps logging & error handling out of SimpleCastClient
                    settings.BeforeCall = call => logger.LogWarning($"Calling {call.Request.RequestUri}");
                    settings.OnError = call => logger.LogError($"Call to SimpleCast failed: {call.Exception}");
                })
                // adds default headers to send with every call
                .WithHeaders(new
                {
                    Accept = "application/json",
                    User_Agent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/75.0.3770.142 Safari/537.36" // Flurl will convert that underscore to a hyphen
                }));

            // Set API url
            Action<ConfigOptions> configOptions = (options =>
            {
                options.base_api_url = Configuration.GetSection("API_url").Value; //Configuration["API_url"];
            });
            services.Configure(configOptions);
            services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<ConfigOptions>>().Value);

            services.AddSingleton<IEmailSender, EmailSender>();
            services.AddResponseCaching();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Intel}/{action=Introduction}/{id?}");
                routes.MapRoute("provinsi", "{controller=Provinsi}/{action=AddProvinsi}/{id?}");
            });

            app.UseResponseCaching();
        }
    }

    public class ApplicationSettings
    {
        public const string SectionKey = "Application";
        public string Name { get; set; }
        public string Flavor { get; set; }
        public string User { get; set; } = "Dr. Codex Lantern";
        public string Email { get; set; } = "drlantern@gotbootstrap.com";
        public string Version { get; set; }
    }
}
