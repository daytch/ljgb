﻿using ljgb.DataAccess.Model;
using ljgb.Common.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using ljgb.Common.Responses;

namespace ljgb.DataAccess.Interface
{
   public interface IWarna
    {
        Task<List<Warna>> GetWarna();

        Task<List<WarnaViewModel>> GetPosts();

        Task<List<WarnaViewModel>> GetAllWithoutFilter(); 

        Task<WarnaViewModel> GetPost(long postId);

        Task<long> Add(Warna warna);

        Task<long> AddPost(Warna warna);

        Task<long> DeletePost(long warnaId);

        Task<bool> UpdatePost(Warna warna);

        Task<List<sp_GetWarnaWithTypeBarang>> GetAllWithTypeBarang(long typeBarangID);
    }
}
