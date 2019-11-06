using System;
using System.Collections.Generic;
using System.Text;

namespace ljgb.Common.ViewModel
{
    public class TypeBarangViewModel
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public long ModelBarangID { get; set; }
        public string NamaModelBarang { get; set; }
        public ModelBarangViewModel ModelBarang { get; set; }
        public long MerkID { get; set; }
        public string NamaMerk { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
        public bool RowStatus { get; set; }
    }
}
