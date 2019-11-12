using ljgb.Common.Requests;
using ljgb.Common.Responses;
using ljgb.Common.ViewModel;
using ljgb.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ljgb.DataAccess.Interface
{
    public interface IMerk
    {
        Task<MerkResponse> GetAll(string search, string order, string orderDir, int startRec, int pageSize, int draw);

        Task<MerkResponse> GetPost(long ID);

        Task<MerkResponse> AddPost(MerkRequest model);

        Task<MerkResponse> DeletePost(long ID);

        Task<MerkResponse> UpdatePost(MerkRequest model);

        Task<MerkResponse> GetAllWithoutFilter();

        List<SP_MerkByKotaID> GetMerkByKotaID(long KotaID);

        Task<Merk> GetMerkByName(string request);
    }
}
