using System;
using System.Collections.Generic;
using System.Text;

namespace ljgb.Common.Responses
{
    public class UserDetailResponse:ResponseBase
    {
        public Int64 Id { get; set; }
        public string Nama { get; set; }
        public string Email { get; set; }
        public string Telp { get; set; }
        public string Alamat { get; set; }
        public string JenisKelamin { get; set; }

        public Int64 ProvinsiId { get; set; }
        public Int64 KotaId { get; set; }
        public string KodeDealer { get; set; }
    }
}
