using ljgb.DataAccess.Models;
using ljgb.DataAccess.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ljgb.DataAccess.Interface
{
    public interface ITrackLevel
    {
        Task<List<TrackLevelViewModel>> GetAll();

        Task<TrackLevelViewModel> GetPost(long ID);

        Task<long> AddPost(TrackLevel model);

        Task<long> DeletePost(long ID);

        Task<bool> UpdatePost(TrackLevel model);
    }
}
