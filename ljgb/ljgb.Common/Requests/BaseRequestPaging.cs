using System;
using System.Collections.Generic;
using System.Text;

namespace ljgb.Common.Requests
{
    public class BaseRequestPaging
    {
        public int start { get; set; }
        public int max { get; set; }
        public int limit { get; set; }
    }
}
