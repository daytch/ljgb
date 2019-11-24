using ljgb.Common.Requests;
using ljgb.Common.Responses;
using ljgb.DataAccess.Interface;
using ljgb.DataAccess.Model;
using ljgb.DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ljgb.BusinessLogic
{
    public class ConfigFacade
    {
        #region Important
        private ljgbContext db;
        private IConfig dataAccess;
        private Security security;

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ConfigFacade()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();
            string connectionString = configuration.GetConnectionString("DefaultConnection").ToString();

            var optionsBuilder = new DbContextOptionsBuilder<ljgbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            security = new Security();

            db = new ljgbContext(optionsBuilder.Options);
            dataAccess = new ConfigRepository(db);
        }
        #endregion

        public async Task<string> GetRedaksionalEmail(string type)
        {
            string result = string.Empty;

            try
            {
                Config config = new Config() { Name = type };
                result = await dataAccess.GetValue(config);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            return result;
        }
    }
}
