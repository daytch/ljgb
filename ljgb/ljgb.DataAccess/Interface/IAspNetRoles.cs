using ljgb.DataAccess.Models;
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

        Task<long> AddPost(AspNetRoles model);

        Task<long> DeletePost(string ID);

        Task<bool> UpdatePost(AspNetRoles model);
    }
}
