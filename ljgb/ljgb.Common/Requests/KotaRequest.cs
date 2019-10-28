﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ljgb.Common.Requests
{
    public class KotaRequest : RequestBase
    {
        public long ID { get; set; }
        public long ProvinsiID { get; set; }
        public string Nama { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime Modified { get; set; }
        public string ModifiedBy { get; set; }
        public bool RowStatus { get; set; }
        
    }
}
