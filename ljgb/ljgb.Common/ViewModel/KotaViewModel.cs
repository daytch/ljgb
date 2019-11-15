using System;
using System.Collections.Generic;
using System.Text;

namespace ljgb.Common.ViewModel
{
    public class KotaViewModel
    {
        public long ID { get; set; }
        public long? ProvinsiID { get; set; }
        public string ProvinsiName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
        public bool RowStatus { get; set; }
        public ProvinsiViewModel Provinsi { get; set; }

        public KotaViewModel()
        {
            Provinsi = new ProvinsiViewModel();
        }

    }
}
