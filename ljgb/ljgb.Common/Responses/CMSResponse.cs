using System;
using System.Collections.Generic;
using System.Text;

namespace ljgb.Common.Responses
{
    public class CMSResponse : BaseResponse
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public List<SP_CMSMaster> List_CMSMaster { get; set; }
        public CMSResponse()
        {
            List_CMSMaster = new List<SP_CMSMaster>();
        }
    }

    public class SP_CMSMaster
    {
        public long ID { get; set; }
        public string Value { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
    }
}
