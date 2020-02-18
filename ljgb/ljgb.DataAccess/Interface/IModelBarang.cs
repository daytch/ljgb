using ljgb.Common.Requests;
using ljgb.Common.Responses;
using ljgb.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ljgb.DataAccess.Interface
{
    public interface IModelBarang
    {
        Task<List<ModelBarang>> GetAllModel();

        Task<ModelBarangResponse> GetAll(string search, string order, string orderDir, int startRec, int pageSize, int draw);

        Task<ModelBarangResponse> GetAllCategory(string search, string order, string orderDir, int startRec, int pageSize, int draw, string type);

        Task<ModelBarang> GetModelBarangByID(long ID);

        Task<long> Add(ModelBarang model);

        Task<ModelBarangResponse> AddPost(ModelBarangRequest model);

        Task<long> DeleteCategory(ModelBarang model);

        Task<long> DeletePost(long ID, string username);

        Task<ModelBarangResponse> UpdatePost(ModelBarangRequest model);

        Task<ModelBarangResponse> UpdateCategory(ModelBarang model);

        Task<ModelBarangResponse> GetModelWithMerkID(ModelBarangRequest model);

        Task<List<SP_ModelByKotaIDMerkID>> GetModelByKotaIDMerkID(ModelBarangRequest model);

        Task<ModelBarang> GetModelWithMerkIDModelName(ModelBarang request);

        //Task<ModelBarangResponse> GetAllWithoutFilter();

    }
}
