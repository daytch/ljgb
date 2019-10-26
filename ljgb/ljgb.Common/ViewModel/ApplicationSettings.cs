using System;
using System.Collections.Generic;
using System.Text;

namespace ljgb.Common.ViewModel
{
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
