using ljgb.Common.ViewModel;
using ljgb.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ljgb.DataAccess.Interface
{
    public interface IBarang
    {
        Task<List<BarangViewModel>> GetAll();

        Task<BarangViewModel> GetPost(long ID);

        Task<long> AddPost(Barang model);

        Task<long> DeletePost(long ID);

        Task<bool> UpdatePost(Barang model);
    }
}
