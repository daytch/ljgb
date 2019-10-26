using ljgb.DataAccess.Model;
using ljgb.DataAccess.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ljgb.DataAccess.Interface
{
    public interface IRole
    {
        Task<List<RoleViewModel>> GetAllRole();



        Task<RoleViewModel> GetPost(long postId);

        Task<long> AddPost(Role role);

        Task<long> DeletePost(long roleID);

        Task<bool> UpdatePost(Role role);
    }
}
