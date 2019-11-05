using ljgb.DataAccess.Interface;
using ljgb.DataAccess.Repository;
using ljgb.DataAccess.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using ljgb.Common.ViewModel;
using ljgb.Common.Responses;
using System.Linq;
using System;

namespace ljgb.BusinessLogic
{
    public class DealerFacade
    {
        #region Important
        private ljgbContext db;
        private IDealer dep;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DealerFacade()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();
            string connectionString = configuration.GetConnectionString("DefaultConnection").ToString();

            var optionsBuilder = new DbContextOptionsBuilder<ljgbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            db = new ljgbContext(optionsBuilder.Options);
            dep = new DealerRepository(db);
        }
        #endregion

        public async Task<List<Dropdown>> GetAllForDropdown(int KotaID)
        {
            List<Dealer> ListDealer = await dep.GetDealerByKotaID(KotaID);
            List<Dropdown> ListDropdown = ListDealer.Select(x => new Dropdown() { ID = x.Id, Text = x.Nama, Code = x.Kode }).ToList();
            if (ListDropdown == null)
            {
                return null;
            }
            return ListDropdown;
        }

    }
}
