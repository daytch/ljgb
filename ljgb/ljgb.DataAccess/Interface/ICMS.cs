using ljgb.Common.Requests;
using ljgb.Common.Responses;
using ljgb.Common.ViewModel;
using ljgb.DataAccess.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ljgb.DataAccess.Interface
{
    public interface ICMS
    {
        Task<List<SP_CMSMaster>> GetAllByCategory(string Category);
        Task<long> SubmitCMS(CMSRequest model);
        Task<List<SP_CMSMaster>> GetAllCMSMaster();
        Task<List<CmsmasterData>> GetAll();
    }
}
