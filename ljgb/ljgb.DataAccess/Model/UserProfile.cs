﻿using System;
using System.Collections.Generic;

namespace ljgb.DataAccess.Model
{
    public partial class UserProfile
    {
        public UserProfile()
        {
            NegoBarang = new HashSet<NegoBarang>();
            ProfileRole = new HashSet<ProfileRole>();
            TransactionBuyer = new HashSet<Transaction>();
            TransactionSeller = new HashSet<Transaction>();
        }

        public long Id { get; set; }
        public string Nama { get; set; }
        public string Email { get; set; }
        public string Telp { get; set; }
        public string Facebook { get; set; }
        public string Ig { get; set; }
        public string JenisKelamin { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
        public bool RowStatus { get; set; }
        public string PhotoPath { get; set; }

        public virtual ICollection<NegoBarang> NegoBarang { get; set; }
        public virtual ICollection<ProfileRole> ProfileRole { get; set; }
        public virtual ICollection<Transaction> TransactionBuyer { get; set; }
        public virtual ICollection<Transaction> TransactionSeller { get; set; }
    }
}
