using ljgb.DataAccess.Models;
using ljgb.DataAccess.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ljgb.DataAccess.Interface
{
   public interface IWarna
    {
        Task<List<Warna>> GetWarna();

        Task<List<WarnaViewModel>> GetPosts();

        Task<WarnaViewModel> GetPost(long postId);

        Task<long> AddPost(Warna warna);

        Task<long> DeletePost(long warnaId);

        Task UpdatePost(Warna warna);
    }
}
