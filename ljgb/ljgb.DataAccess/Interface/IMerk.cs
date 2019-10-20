using ljgb.DataAccess.Models;
using ljgb.DataAccess.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ljgb.DataAccess.Interface
{
    public interface IMerk
    {
        Task<List<MerkViewModel>> GetAll();

        Task<MerkViewModel> GetPost(long ID);

        Task<long> AddPost(Merk model);

        Task<long> DeletePost(long ID);

        Task<bool> UpdatePost(Merk model);
    }
}
