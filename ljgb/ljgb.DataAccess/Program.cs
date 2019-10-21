using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace ljgb.DataAccess
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IServiceCollection services = new ServiceCollection();
            // Startup.cs finally :)
            Startup startup = new Startup();
            startup.ConfigureServices(services);
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            //configure console logging
            //serviceProvider
            //    .GetService<ILoggerFactory>()
            //    .AddConsole(LogLevel.Debug);

            //var logger = serviceProvider.GetService<ILoggerFactory>()
            //    .CreateLogger<Program>();
            
            // Get Service and call method
            //var service = serviceProvider.GetService<IMyService>();
        }
    }
}
