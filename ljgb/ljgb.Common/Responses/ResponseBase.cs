using System;
using System.Collections.Generic;
using System.Text;

namespace ljgb.Common.Responses
{
    public class ResponseBase
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public ResponseBase()
        {
            IsSuccess = true;
        }
    }
}
