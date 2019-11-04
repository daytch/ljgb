using ljgb.Common.Requests;
using ljgb.Common.Responses;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ljgb.DataAccess.Interface
{
    public interface IModelBarang
    {
        Task<ModelBarangResponse> GetAll(string search, string order, string orderDir, int startRec, int pageSize, int draw);

        Task<ModelBarangResponse> GetPost(long ID);

        Task<ModelBarangResponse> AddPost(ModelBarangRequest model);

        Task<ModelBarangResponse> DeletePost(ModelBarangRequest model);

        Task<ModelBarangResponse> UpdatePost(ModelBarangRequest model);

        Task<ModelBarangResponse> GetModelWithMerkID(ModelBarangRequest model);

        //Task<ModelBarangResponse> GetAllWithoutFilter();
        
    }
}
