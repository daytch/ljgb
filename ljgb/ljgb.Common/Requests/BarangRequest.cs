
using ljgb.Common.ViewModel;
using System;

namespace ljgb.Common.Requests
{
    public class BarangRequest: BaseRequestPaging
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public long HargaOtr { get; set; }
        public long WarnaId { get; set; }
        public WarnaViewModel WarnaBarang { get; set; }
        public long TypeBarangId { get; set; }
        public TypeBarangViewModel TypeBarang { get; set; }

        public long LowestAsk { get; set; }
        public long HighestBid { get; set; }
        public string Description { get; set; }

        public string PhotoPath { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
        public bool RowStatus { get; set; }
    }
}
