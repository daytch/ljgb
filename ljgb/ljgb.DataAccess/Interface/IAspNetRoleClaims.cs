﻿using ljgb.DataAccess.Model;
using ljgb.DataAccess.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ljgb.DataAccess.Interface
{
    public interface IAspNetRoleClaims
    {
        Task<List<AspNetRoleClaimsViewModel>> GetAll();

        Task<AspNetRoleClaimsViewModel> GetPost(long ID);

        Task<long> AddPost(AspNetRoleClaims model);

        Task<long> DeletePost(long ID);

        Task<bool> UpdatePost(AspNetRoleClaims model);
    }
}
