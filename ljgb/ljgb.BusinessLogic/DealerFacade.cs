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
using ljgb.Common.Requests;

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
            List<Dropdown> ListDropdown = ListDealer.Select(x => new Dropdown() { ID = x.Id, Text = x.Name, Code = x.Kode }).ToList();
            if (ListDropdown == null)
            {
                return null;
            }
            return ListDropdown;
        }

        public async Task<DealerResponse> GetAll(string search, string order, string orderDir, int startRec, int pageSize, int draw)
        {
            return await dep.GetAll(search, order, orderDir, startRec, pageSize, draw);
        }

        public async Task<DealerResponse> AddPost(DealerRequest request)
        {
            DealerResponse response = new DealerResponse();
            try
            {
                Dealer model = new Dealer();
                model.Name = request.Name;
                model.Kode = request.Kode;
                model.Alamat = request.Alamat;
                model.KotaId = request.KotaID;
                model.Telepon = request.Telepon;
                model.PejabatDealer = request.PejabatDealer;
                model.Created = DateTime.Now;
                model.CreatedBy = request.UserName;
                model.RowStatus = true;

                long result = 0;
                result = await dep.AddPost(model);
                if (result>0)
                {
                    response.Message = "success";
                    response.IsSuccess = true;
                }
                else
                {
                    response.Message = "failed";
                    response.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {

                response.Message = ex.ToString();
                response.IsSuccess = false;
            }
            return response;
        }

        //public async Task<DealerResponse> GetPost(long request)
        //{
        //    DealerResponse response = new DealerResponse();
            
        //    return response;
        //}

        public async Task<DealerResponse> UpdatePost(DealerRequest request)
        {
            DealerResponse response = new DealerResponse();
            
            try
            {
                Dealer dealer = await dep.GetPost(request.ID);
                dealer.KotaId = request.KotaID;
                dealer.Kode = request.Kode;
                dealer.Name = request.Name;
                dealer.Alamat = request.Alamat;
                dealer.Telepon = request.Telepon;
                dealer.PejabatDealer = request.PejabatDealer;
                dealer.Modified = DateTime.Now;
                dealer.ModifiedBy = request.UserName;

                if (await dep.UpdatePost(dealer))
                {

                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Update Failed";
                }
            }
            catch (Exception ex)
            {

                response.IsSuccess = false;
                response.Message = ex.ToString();
            }

            return response;
        }

        public async Task<DealerResponse> DeletePost(long ID, string username)
        {
            DealerResponse response = new DealerResponse();
            try
            {
                if (await dep.DeletePost(ID, username) > 0)
                {
                    response.IsSuccess = true;
                    response.Message = "Delete Success";
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Delete Failed";
                }
            }
            catch (Exception ex)
            {

                response.IsSuccess = false;
                response.Message = ex.ToString();
            }

            return response;
        }

    }
}
