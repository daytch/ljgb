using ljgb.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ljgb.Common.Responses
{
    public class NegoBarangResponse : ResponseBase
    {
        public List<NegoBarangViewModel> ListModel { get; set; }
        public NegoBarangResponse()
        {
            IsSuccess = true;
            ListModel = new List<NegoBarangViewModel>();
        }
    }
}
