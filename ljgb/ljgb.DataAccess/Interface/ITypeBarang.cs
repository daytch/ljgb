using ljgb.DataAccess.Models;
using ljgb.DataAccess.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ljgb.DataAccess.Interface
{
    public interface ITypeBarang
    {

        Task<List<TypeBarangViewModel>> GetAll();

        Task<TypeBarangViewModel> GetPost(long ID);

        Task<long> AddPost(TypeBarang model);

        Task<long> DeletePost(long ID);

        Task<bool> UpdatePost(TypeBarang model);
    }
}
