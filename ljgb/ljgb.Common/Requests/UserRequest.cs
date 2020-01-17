using Microsoft.AspNetCore.Identity;
using System;

namespace ljgb.Common.Requests
{
    public class UserRequest : BaseRequest
    {
        public IdentityUser user { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string HTMLTag { get; set; }
        public string EmailSubject { get; set; }
        public bool RememberMe { get; set; }
        public bool lockoutOnFailure { get; set; }

        public int DetailID { get; set; }
        public string VerifiedBy { get; set; }
        public DateTime VerifiedDate { get; set; }


        public int Id { get; set; }
        public string Nama { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string RePassword { get; set; }
        public string Telp { get; set; }
        public string Alamat { get; set; }
        public string JenisKelamin { get; set; }
        public int KotaId { get; set; }
        public string KodeDealer { get; set; }
        public string Facebook { get; set; }
        public string Instagram { get; set; }
        public string Photopath { get; set; }
        public bool IsAdminPortal { get; set; }
        public bool IsActive { get; set; }
    }

    public class sp_GetUserDetail
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
