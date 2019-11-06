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

        public List<SP_MerkByKotaID> ListSP_MerkByKotaID { get; set; }
        public MerkViewModel Model { get; set; }

        public MerkResponse()
        {
            ListModel = new List<MerkViewModel>();
        }
    }

    public class SP_MerkByKotaID
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
