using ljgb.Common.Requests;
using ljgb.Common.Responses;
using ljgb.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ljgb.DataAccess.Interface
{
    public interface IProvinsi
    {
        Task<List<Provinsi>> GetAllForDropdown();

        Task<ProvinsiResponse> GetAll();

        Task<ProvinsiResponse> GetPost(ProvinsiRequest request);

        Task<ProvinsiResponse> AddPost(ProvinsiRequest request);

        Task<ProvinsiResponse> DeletePost(ProvinsiRequest request);

        Task<ProvinsiResponse> UpdatePost(ProvinsiRequest request);
    }
}
