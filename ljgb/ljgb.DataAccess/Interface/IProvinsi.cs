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
        Task<ProvinsiResponse> GetAll(string search, string order, string orderDir, int startRec, int pageSize, int draw);
        Task<List<Provinsi>> GetAllForDropdown();

        Task<ProvinsiResponse> GetAll();

        Task<ProvinsiResponse> GetPost(ProvinsiRequest request);

        Task<Provinsi> GetPostByID(long ID);

        Task<long> AddPost(Provinsi request);

        Task<ProvinsiResponse> DeletePost(ProvinsiRequest request);

        Task<long> UpdatePost(Provinsi request);
    }
}
