using ljgb.DataAccess.Models;
using ljgb.DataAccess.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ljgb.DataAccess.Interface
{
    public interface IHargaSalesman
    {
        Task<List<HargaSalesmanViewModel>> GetAll();

        Task<HargaSalesmanViewModel> GetPost(long ID);

        Task<long> AddPost(HargaSalesman model);

        Task<long> DeletePost(long ID);

        Task<bool> UpdatePost(HargaSalesman model);
    }
}
