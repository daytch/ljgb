using ljgb.Common.Requests;
using ljgb.Common.Responses;
using ljgb.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ljgb.DataAccess.Interface
{
    public interface INegoBarang
    {
        Task<NegoBarangResponse> GetAll();

        Task<NegoBarangResponse> GetPost(NegoBarangRequest model);

        Task<NegoBarangResponse> AddPost(NegoBarangRequest model);

        Task<NegoBarangResponse> DeletePost(NegoBarangRequest model);

        Task<NegoBarangResponse> UpdatePost(NegoBarangRequest model);
    }
}
