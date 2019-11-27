using ljgb.Common.ViewModel;
using System.Collections.Generic;

namespace ljgb.Common.Responses
{
    public class WarnaResponse : ResponseBase
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public List<WarnaViewModel> data { get; set; }
        public List<sp_GetWarnaWithTypeBarang> dataWithTypeBarang { get; set; }
    }

    public class sp_GetWarnaWithTypeBarang
    {
        public long ID { get; set; }
        public string Name { get; set; }
    }
}
