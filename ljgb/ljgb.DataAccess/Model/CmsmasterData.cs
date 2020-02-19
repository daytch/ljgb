﻿using System;
using System.Collections.Generic;

namespace ljgb.DataAccess.Model
{
    public partial class CmsmasterData
    {
        public long Id { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
        public bool RowStatus { get; set; }
    }
}
