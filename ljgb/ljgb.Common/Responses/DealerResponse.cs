using ljgb.Common.ViewModel;
using System.Collections.Generic;

namespace ljgb.Common.Responses
{
    public class DealerResponse : ResponseBase
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public List<DealerViewModel> ListModel { get; set; }
        public DealerViewModel Model { get; set; }
        public List<Dropdown> ListDealer { get; set; }

        public DealerResponse()
        {
            ListModel = new List<DealerViewModel>();
            Model = new DealerViewModel();
        }
    }
}
