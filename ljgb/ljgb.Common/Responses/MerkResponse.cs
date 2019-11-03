using ljgb.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ljgb.Common.Responses
{
    public class MerkResponse : BaseResponse
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public List<MerkViewModel> ListModel { get; set; }

        public MerkViewModel Model { get; set; }

        public MerkResponse()
        {
            ListModel = new List<MerkViewModel>();
        }
    }
}
