using ljgb.Common.Requests;
using ljgb.Common.Responses;
using ljgb.Common.ViewModel;
using ljgb.DataAccess.Interface;
using ljgb.DataAccess.Model;
using ljgb.DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ljgb.BusinessLogic
{
    public class CMSFacade
    {
        #region Important
        private ljgbContext db;
        private ICMS dep;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public CMSFacade()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();
            string connectionString = configuration.GetConnectionString("DefaultConnection").ToString();

            var optionsBuilder = new DbContextOptionsBuilder<ljgbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            db = new ljgbContext(optionsBuilder.Options);
            this.dep = new CMSRepository(db);
        }
        #endregion
        public async Task<CMSResponse> GetAllBanner()
        {

            CMSResponse response = new CMSResponse();

            response.List_CMSMaster = await dep.GetAllByCategory("Banner");


            return response;

        }

        public async Task<CMSResponse> GetAllMaster()
        {
            CMSResponse response = new CMSResponse();
            
          
            try
            {
                response.List_CMSMaster = await dep.GetAllCMSMaster();
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {

                response.IsSuccess = false;
                response.Message = ex.Message.ToString();
            }
            return response;

        }

        

        public async Task<CMSResponse> Submit(CMSRequest req)
        {
            CMSResponse response = new CMSResponse();
            try
            {
                //List<CmsmasterData> models = await dep.GetAll();
                //List<CmsmasterData> listDataSubmit = PopulateCMSMasterBeforeSubmit(models, req);

                var result = await dep.SubmitCMS(req);
                if (result > 0)
                {
                    response.Message = "Success";
                    response.IsSuccess = true;
                }
                else
                {
                    response.Message = "Failed";
                    response.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                response.Message = "Failed";
                response.IsSuccess = false;
            }
            return response;
        }

        //private List<CmsmasterData> PopulateCMSMasterBeforeSubmit(List<CmsmasterData> models, CMSRequest req)
        //{
        //    List<CmsmasterData> result = new List<CmsmasterData>();
        //    foreach (var item in req.List_CMSMaster)
        //    {
        //        if (models.Any(x=>x.Description == item.Description))
        //        {
        //            CmsmasterData data = new CmsmasterData();
        //            data = models.Where(x => x.Description == item.Description).First();
        //            data.Modified =  DateTime.Now;
        //            data.ModifiedBy = req.Username;
        //            data.Value = item.Value;
        //            result.Add(data);
        //        }
        //    }

        //    return result;
        //}
    }
}
