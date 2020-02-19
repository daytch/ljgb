using ljgb.Common.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace ljgb.Common.Requests
{
    public class CMSRequest : BaseRequest
    {
        public long ID { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public string Username { get; set; }
        public string Banner1 { get; set; }
        public string Category { get; set; }
        public List<SP_CMSMaster> List_CMSMaster { get; set; }
    }
}
