using ljgb.Common.Requests;
using ljgb.Common.Responses;
using ljgb.DataAccess.Model;
using System.Threading.Tasks;

namespace ljgb.DataAccess.Interface
{
    public interface INegoBarang
    {
        Task<NegoBarangResponse> GetAll();

        Task<NegoBarangResponse> GetPost(NegoBarangRequest model);

        Task<NegoBarangResponse> AddPost(NegoBarangRequest model);

        Task<NegoBarang> GetNegoBarang(NegoBarang model);

        Task<NegoBarangResponse> DeletePost(NegoBarangRequest model);

        Task<NegoBarangResponse> UpdatePost(NegoBarangRequest model);
    }
}
