using ljgb.DataAccess.Models;
using ljgb.DataAccess.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ljgb.DataAccess.ViewModel;

namespace ljgb.DataAccess.Interface
{
    public interface IWilayah
    {
        Task<List<WilayahViewModel>> GetAllWilayah();

        Task<WilayahViewModel> GetPost(long postId);

        Task<long> AddPost(Wilayah wilayah);

        Task<long> DeletePost(long roleID);

        Task<bool> UpdatePost(Wilayah wilayah);
    }
}
