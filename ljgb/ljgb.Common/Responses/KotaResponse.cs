using ljgb.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ljgb.Common.Responses
{
    public class KotaResponse : ResponseBase
    {
        public KotaViewModel Model { get; set; }
        public List<KotaViewModel> ListKota { get; set; }
        public List<ProvinsiViewModel> ListProvinsi { get; set; }

        public KotaResponse()
        {
            Model = new KotaViewModel();
            ListKota = new List<KotaViewModel>();
            ListProvinsi = new List<ProvinsiViewModel>();
        }
    }
}
