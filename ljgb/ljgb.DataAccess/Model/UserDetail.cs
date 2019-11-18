using System;
using System.Collections.Generic;

namespace ljgb.DataAccess.Model
{
    public partial class UserDetail
    {
        public int Id { get; set; }
        public int UserProfileId { get; set; }
        public DateTime? VerifiedDate { get; set; }
        public string VerifiedBy { get; set; }
        public string Description { get; set; }
        public string Photo { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
        public bool RowStatus { get; set; }
        public string KodeDealer { get; set; }
    }
}
