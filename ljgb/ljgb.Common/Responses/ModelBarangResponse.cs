using ljgb.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ljgb.Common.Responses
{
    public class ModelBarangResponse : ResponseBase
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public List<ModelBarangViewModel> ListModel { get; set; }
        public ModelBarangViewModel Model { get; set; }

        public ModelBarangResponse()
        {
            ListModel = new List<ModelBarangViewModel>();
            Model = new ModelBarangViewModel();
        }
    }
}
