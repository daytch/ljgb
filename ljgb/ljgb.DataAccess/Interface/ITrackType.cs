using ljgb.DataAccess.Models;
using ljgb.DataAccess.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ljgb.DataAccess.Interface
{
    public interface ITrackType
    {
        Task<List<TrackTypeViewModel>> GetAll();

        Task<TrackTypeViewModel> GetPost(long ID);

        Task<long> AddPost(TrackType model);

        Task<long> DeletePost(long ID);

        Task<bool> UpdatePost(TrackType model);
    }
}
