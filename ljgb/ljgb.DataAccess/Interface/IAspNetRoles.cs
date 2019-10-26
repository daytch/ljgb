using ljgb.DataAccess.Model;
using ljgb.DataAccess.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ljgb.DataAccess.Interface
{
    public interface IAspNetRoles
    {
        Task<List<AspNetRolesViewModel>> GetAll();

        Task<AspNetRolesViewModel> GetPost(long ID);

        Task<string> AddPost(AspNetRoles model);

        Task<string> DeletePost(string ID);

        Task<bool> UpdatePost(AspNetRoles model);
    }
}
