using System;
using System.Collections.Generic;
using System.Text;

namespace ljgb.Common.ViewModel
{
    public class TransactionViewModel
    {
        public long ID { get; set; }
        public UserProfileViewModel Buyer { get; set; }
        public UserProfileViewModel Seller { get; set; }
        public NegoBarangViewModel NegoBarang { get; set; }
        public TransactionLevelViewModel TrasanctionLevel { get; set; }




        public string NamaDealer { get; set; }
        public string NamaBarang { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
        public bool RowStatus { get; set; }

        public string NamaStatus { get; set; }
        public string NamaLangkah { get; set; }
        public long HargaOTR { get; set; }
        public long HargaNego { get; set; }
        public string NamaPembeli { get; set; }
        public string NamaPenjual { get; set; }
        public long IDPembeli { get; set; }
        public long IDPenjual { get; set; }
        public string EmailPembeli { get; set; }
        public string EmailPenjual { get; set; }
        public string TelpPembeli { get; set; }
        public string TelpPenjual { get; set; }
        public string AlamatPembeli { get; set; }
        public string AlamatPenjual { get; set; }
        public string FacebookPembeli { get; set; }
        public string FacebookPenjual { get; set; }
        public string IGPembeli { get; set; }
        public string IGPenjual { get; set; } 


        public string NamaDealerKota { get; set; }
        public string KodeDealer { get; set; }
        public string AlamatDealer { get; set; }
        public string KotaDealer { get; set; }
        public string PejabatDealer { get; set; }
        public string TelpDealer { get; set; }
        public string TipePenawaran { get; set; }

        public TransactionViewModel()
        {
            Buyer = new UserProfileViewModel();
            Seller = new UserProfileViewModel();
            NegoBarang = new NegoBarangViewModel();
            TrasanctionLevel = new TransactionLevelViewModel();
        }
      
    }
    public class TransactionHistoryViewModel
    {
        public string TransactionStatus { get; set; }
        public string TransactionStep { get; set; }
        public DateTime Created { get; set; }
    }
}
