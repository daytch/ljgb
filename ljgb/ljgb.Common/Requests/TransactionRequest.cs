using ljgb.Common.ViewModel;
using System;
using System.Collections.Generic;

namespace ljgb.Common.Requests
{
    public class TransactionRequest : RequestBase
    {
        public long ID { get; set; }
        public UserProfileViewModel Buyer { get; set; }
        public UserProfileViewModel Seller { get; set; }
        public NegoBarangViewModel NegoBarang { get; set; }
        public TransactionLevelViewModel TrasanctionLevel { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
        public bool RowStatus { get; set; }
        public string draw { get; set; }
        public string search { get; set; }
        public List<string> order { get; set; }
        public string start { get; set; }
        public string length { get; set; }

        public long BuyerID { get; set; }
        public long SellerID { get; set; }
        public long NegoBarangID { get; set; }

        public long TransactionStatusID { get; set; }
        public long UserProfileID { get; set; }
        public string EndDate { get; set; }
        public string UserName { get; set; }
  
        public long BarangID { get; set; }
        public long Nominal { get; set; }

        public long Harga { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Telp { get; set; }
        public string Alamat { get; set; }
        public string Facebook { get; set; }
        public string Instagram { get; set; }

        public TransactionRequest()
        {
            TrasanctionLevel = new TransactionLevelViewModel();
        }
    }


}
