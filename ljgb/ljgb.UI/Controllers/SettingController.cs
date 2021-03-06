﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ljgb.UI.Controllers
{
    public class SettingsController : BaseController
    {
        private readonly ApplicationSettings _settings;

        public SettingsController(IOptions<ApplicationSettings> settings)
        {
            _settings = settings.Value;
        }
        public IActionResult HowItWorks() => View(_settings);
        public IActionResult LayoutOptions() => View(_settings);
        public IActionResult SkinOptions() => View(_settings);
        public IActionResult SavingDb() => View(_settings);
    }
}
