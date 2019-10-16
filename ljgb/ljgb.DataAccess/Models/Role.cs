﻿using System;
using System.Collections.Generic;

namespace ljgb.DataAccess.Models
{
    public partial class Role
    {
        public Role()
        {
            ProfileRole = new HashSet<ProfileRole>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
        public bool RowStatus { get; set; }

        public ICollection<ProfileRole> ProfileRole { get; set; }
    }
}
