using ljgb.Common.Requests;
using ljgb.Common.Responses;
using ljgb.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ljgb.DataAccess.Interface
{
    public interface ITypeBarang
    {

        Task<TypeBarangResponse> GetAll(string search, string order, string orderDir, int startRec, int pageSize, int draw);
        Task<TypeBarangResponse> GetAllWithModelID(TypeBarangRequest request);

        Task<TypeBarangResponse> GetPost(TypeBarangRequest request);

        Task<TypeBarangResponse> AddPost(TypeBarang request);

        Task<TypeBarangResponse> DeletePost(TypeBarangRequest request);

        Task<TypeBarangResponse> UpdatePost(TypeBarangRequest request);

        Task<List<SP_TypeByKotaIDMerkIDModelID>> GetTypeByKotaIDMerkIDModelID(TypeBarangRequest request);

        Task<TypeBarang> GetTypeByName(TypeBarang request);
    }
}
