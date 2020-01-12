using ljgb.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ljgb.Common.Responses
{
    public class TransactionResponse : ResponseBase
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public List<TransactionViewModel> ListTransaction { get; set; }

        public List<TransactionJournalViewModel> ListJournalByTransaction { get; set; }
        public List<TransactionHistoryViewModel> ListHistory { get; set; }
        public List<TransactionStatusViewModel> ListStatus { get; set; }
        public List<SP_GetAllAskByUserProfileID> ListSP_GetAllAskByUserProfileID { get; set; }
        public List<SP_GetAllBidByUserProfileID> ListSP_GetAllBidByUserProfileID { get; set; }

        public List<SP_ReportByStatusID> listSP_ReportByStatusID { get; set; }

        public List<sp_GetAllBidAndBuyByUserProfileID> ListBidAndBuy { get; set; }

        public TransactionResponse()
        {
            ListTransaction = new List<TransactionViewModel>();
            ListStatus = new List<TransactionStatusViewModel>();
        }

    }

    public class SP_ReportByStatusID
    {
        public long id { get; set; }
        public DateTime TransactionCreated { get; set; }
        public string TransactionCreatedBy { get; set; }
        public DateTime? TransactionModified { get; set; }
        public string TransactionModifiedBy { get; set; }
        public long BuyerID { get; set; }
        public string BuyerNama { get; set; }
        public string BuyerEmail { get; set; }
        public string BuyerTelp { get; set; }
        public string BuyerFacebook { get; set; }
        public string BuyerIG { get; set; }
        public string BuyerPhoto { get; set; }
        public string BuyerJenisKelamin { get; set; }
        public string BuyerAlamat { get; set; }
        public string BuyerKota { get; set; }
        public long SellerID { get; set; }
        public string SellerNama { get; set; }
        public string SellerEmail { get; set; }
        public string SellerTelp { get; set; }
        public string SellerFacebook { get; set; }
        public string SellerIG { get; set; }
        public string SellerPhoto { get; set; }
        public string SellerJenisKelamin { get; set; }
        public string SellerVerifiedBy { get; set; }
        public string SellerAlamat { get; set; }
        public string SellerKota { get; set; }
        public string SellerDealerKode { get; set; }
        public string DealerAlamat { get; set; }
        public string DealerKota { get; set; }
        public string DealerTelp { get; set; }
        public string DealerPejabat { get; set; }
        public string NegoType { get; set; }
        public long? NegoHarga { get; set; }
        public DateTime NegoCreated { get; set; }
        public long? BarangOTR { get; set; }
        public string BarangMerk { get; set; }
        public string BarangModel { get; set; }
        public string BarangType { get; set; }
        public string BarangWarna { get; set; }
        public string BarangKota { get; set; }
        public string TransactionStatus { get; set; }

    }

    public class SP_GetAllAskByUserProfileID
    {
        public long id { get; set; }
        public string merkName { get; set; }
        public string modelName { get; set; }
        public string colour { get; set; }
        public string typePenawaran { get; set; }
        public long price { get; set; }
    }
    public class SP_GetAllBidByUserProfileID
    {
        public long id { get; set; }
        public string merkName { get; set; }
        public string modelName { get; set; }
        public string colour { get; set; }
        public string typePenawaran { get; set; }
        public long price { get; set; }
    }

    public class sp_GetAllBidAndBuyByUserProfileID
    {
        public long ID { get; set; }
        public long NegoBarangID { get; set; }
        public string MerkName { get; set; }
        public string ModelName { get; set; }
        public string TypeName { get; set; }
        public long BarangID { get; set; }
        public string BarangName { get; set; }
        public long Price { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }
        public string Colour { get; set; }
    }
}
