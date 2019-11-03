using System;
using System.Collections.Generic;
using System.Text;

namespace ljgb.Common.ViewModel
{
    public class BarangViewModel
    {
        public long Id { get; set; }
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
