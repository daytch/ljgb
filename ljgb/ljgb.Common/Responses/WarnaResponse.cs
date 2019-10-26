using ljgb.Common.ViewModel;
using System.Collections.Generic;

namespace ljgb.Common.Responses
{
    public class WarnaResponse
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public List<WarnaViewModel> data { get; set; }
    }
}
