using ljgb.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ljgb.Common.Responses
{
    public class TypeBarangResponse : ResponseBase
    {

        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public List<TypeBarangViewModel> ListModel { get; set; }
        public TypeBarangViewModel Model { get; set; }

        public TypeBarangResponse()
        {
            ListModel = new List<TypeBarangViewModel>();
            Model = new TypeBarangViewModel();
        }
    }
}
