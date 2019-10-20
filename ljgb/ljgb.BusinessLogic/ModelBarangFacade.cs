﻿using ljgb.DataAccess.Interface;
using ljgb.DataAccess.Models;
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
    public class ModelBarangFacade
    {
        #region Important
        private ljgbContext db;
        private IModelBarang dep;

        public ModelBarangFacade()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();
            string connectionString = configuration.GetConnectionString("DefaultConnection").ToString();

            var optionsBuilder = new DbContextOptionsBuilder<ljgbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            db = new ljgbContext(optionsBuilder.Options);
            this.dep = new ModelBarangRepository(db);
        }
        #endregion


        public async Task<List<ModelBarangViewModel>> GetAll()
        {
            var models = await dep.GetAll();
            if (models == null)
            {
                return null;
            }
            return models;
        }



        public async Task<ModelBarangViewModel> GetPost(long ID)
        {
            var model = await dep.GetPost(ID);

            if (model == null)
            {
                return null;
            }
            return model;

        }

        public async Task<long> AddPost(ModelBarang model)
        {
            var postId = await dep.AddPost(model);
            if (postId > 0)
            {
                return postId;
            }
            else
            {
                return 0;
            }
        }

        public async Task<long> DeletePost(long ID)
        {
            long result = 0;
            result = await dep.DeletePost(ID);
            if (result == 0)
            {
                return 0;
            }
            return result;
        }

        public async Task<bool> UpdatePost(ModelBarang model)
        {
            bool result = await dep.UpdatePost(model);

            return result;
        }
    }
}
