using ljgb.DataAccess.Interface;
using ljgb.DataAccess.Model;
using ljgb.DataAccess.Repository;
using ljgb.DataAccess.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ljgb.BusinessLogic
{
    public class TrackStatusFacade
    {
        //#region Important
        //private ljgbContext db;
        //private ITrackStatus dep;

        //public TrackStatusFacade()
        //{
        //    var builder = new ConfigurationBuilder()
        //        .SetBasePath(Directory.GetCurrentDirectory())
        //        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        //    IConfigurationRoot configuration = builder.Build();
        //    string connectionString = configuration.GetConnectionString("DefaultConnection").ToString();

        //    var optionsBuilder = new DbContextOptionsBuilder<ljgbContext>();
        //    optionsBuilder.UseSqlServer(connectionString);

        //    db = new ljgbContext(optionsBuilder.Options);
        //    this.dep = new TrackStatusRepository(db);
        //}
        //#endregion


        //public async Task<List<TrackStatusViewModel>> GetAll()
        //{
        //    var models = await dep.GetAll();
        //    if (models == null)
        //    {
        //        return null;
        //    }
        //    return models;
        //}



        //public async Task<TrackStatusViewModel> GetPost(long ID)
        //{
        //    var model = await dep.GetPost(ID);

        //    if (model == null)
        //    {
        //        return null;
        //    }
        //    return model;

        //}

        //public async Task<long> AddPost(TrackStatus model)
        //{
        //    var postId = await dep.AddPost(model);
        //    if (postId > 0)
        //    {
        //        return postId;
        //    }
        //    else
        //    {
        //        return 0;
        //    }
        //}

        //public async Task<long> DeletePost(long ID)
        //{
        //    long result = 0;
        //    result = await dep.DeletePost(ID);
        //    if (result == 0)
        //    {
        //        return 0;
        //    }
        //    return result;
        //}

        //public async Task<bool> UpdatePost(TrackStatus model)
        //{
        //    bool result = await dep.UpdatePost(model);

        //    return result;
        //}
    }
}
