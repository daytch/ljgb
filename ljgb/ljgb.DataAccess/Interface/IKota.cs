﻿using ljgb.Common.Requests;
using ljgb.Common.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ljgb.DataAccess.Interface
{
    public interface IKota
    {
        Task<KotaResponse> GetAll(string search, string order, string orderDir, int startRec, int pageSize, int draw);

        Task<KotaResponse> GetPost(KotaRequest request);

        Task<KotaResponse> AddPost(KotaRequest request);

        Task<KotaResponse> DeletePost(KotaRequest request);

        Task<KotaResponse> UpdatePost(KotaRequest request);
    }
}
