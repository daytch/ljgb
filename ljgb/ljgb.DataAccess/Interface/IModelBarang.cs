using ljgb.DataAccess.Models;
using ljgb.DataAccess.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ljgb.DataAccess.Interface
{
    public interface IModelBarang
    {
        Task<List<ModelBarangViewModel>> GetAll();

        Task<ModelBarangViewModel> GetPost(long ID);

        Task<long> AddPost(ModelBarang model);

        Task<long> DeletePost(long ID);

        Task<bool> UpdatePost(ModelBarang model);
    }
}
