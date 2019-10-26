using ljgb.Common.Requests;
using ljgb.Common.Responses;
using ljgb.DataAccess.Interface;
using ljgb.DataAccess.Model;
using ljgb.DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ljgb.BusinessLogic
{
    public class TransactionLevelFacade
    {
        #region Important
        private ljgbContext db;
        private ITransactionLevel dep;

        public TransactionLevelFacade()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();
            string connectionString = configuration.GetConnectionString("DefaultConnection").ToString();

            var optionsBuilder = new DbContextOptionsBuilder<ljgbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            db = new ljgbContext(optionsBuilder.Options);
            this.dep = new TransactionLevelRepository(db);
        }
        #endregion

        public async Task<TransactionLevelResponse> GetAll()
        {
            var models = await dep.GetAll();
            if (models == null)
            {
                return null;
            }
            return models;
        }

        public async Task<TransactionLevelResponse> GetCurrentLevel(TransactionLevelRequest request)
        {

            var models = await dep.GetCurrentLevel(request);
            if (models == null)
            {
                return null;
            }
            return models;
        }

        public async Task<TransactionLevelResponse> GetNextLevel(TransactionLevelRequest request)
        {


            return await dep.GetNextLevel(request);
        }
        
    }
        
    
}
