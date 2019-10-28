using System;
using System.Collections.Generic;

namespace ljgb.DataAccess.Model
{
    public partial class UserDetail
    {
        public int Id { get; set; }
        public int UserProfileId { get; set; }
<<<<<<< HEAD
        public string KodeDealer { get; set; }
=======
>>>>>>> c26e61f71be20fe3bfd2c1703d7dba5fda547bb3
        public DateTime? VerifiedDate { get; set; }
        public string VerifiedBy { get; set; }
        public string Description { get; set; }
        public string Photo { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
        public bool RowStatus { get; set; }
    }
}
