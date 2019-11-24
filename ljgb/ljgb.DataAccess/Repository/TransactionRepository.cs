using ljgb.Common.Requests;
using ljgb.Common.Responses;
using ljgb.DataAccess.Interface;
using ljgb.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ljgb.Common.ViewModel;

namespace ljgb.DataAccess.Repository
{
    public class TransactionRepository : ITransaction
    {
        ljgbContext db;
        public TransactionRepository(ljgbContext _db)
        {
            db = _db;
        }

        public async Task<long> SaveTransaction(Transaction model)
        {
            long TransactionID = 0;
            if (db != null)
            {
                try
                {
                    await db.Transaction.AddAsync(model);

                    await db.SaveChangesAsync();
                    TransactionID = model.Id;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
            return TransactionID;
        }

        public async Task<Transaction> Select(Transaction trans)
        {
            Transaction result = new Transaction();
            if (db != null)
            {
                result = await db.Transaction.FirstOrDefaultAsync(x => x.Id == trans.Id && x.RowStatus == true);
                return result;
            }
            return null;
        }

        public async Task<bool> Update(Transaction trans)
        {
            bool result = false;
            if (db != null)
            {
                db.Transaction.Update(trans);

                //Commit the transaction
                await db.SaveChangesAsync();
                result = true;
            }
            return result;
        }

        public async Task<TransactionResponse> AddPost(TransactionRequest model)
        {
            TransactionResponse response = new TransactionResponse();


            if (db != null)
            {
                try
                {

                    Transaction transaction = new Transaction();


                    transaction.BuyerId = model.Buyer.ID;
                    transaction.SellerId = model.Seller.ID;
                    transaction.NegoBarangId = model.NegoBarang.ID;
                    transaction.TransactionLevelId = model.TrasanctionLevel.ID;
                    transaction.Created = DateTime.Now;
                    transaction.CreatedBy = model.CreatedBy;
                    transaction.RowStatus = true;
                    await db.Transaction.AddAsync(transaction);


                    TransactionJournal journal = new TransactionJournal();
                    journal.TransactionId = transaction.Id;
                    journal.BuyerId = transaction.BuyerId;
                    journal.SellerId = transaction.SellerId;
                    journal.NegoBarangId = transaction.NegoBarangId;
                    journal.TransactionLevelId = transaction.TransactionLevelId;
                    journal.Created = transaction.Created;
                    journal.CreatedBy = transaction.CreatedBy;
                    journal.RowStatus = true;

                    await db.TransactionJournal.AddAsync(journal);

                    await db.SaveChangesAsync();

                    response.Message = "Data has been saved!";
                }
                catch (Exception ex)
                {

                    response.IsSuccess = false;
                    response.Message = ex.ToString();
                }
            }


            return response;
        }

        public async Task<TransactionResponse> ApproveTransaction(TransactionRequest req)
        {
            TransactionResponse response = new TransactionResponse();

            if (db != null)
            {
                try
                {
                    Transaction model = await db.Transaction.Where(x => x.RowStatus == true && x.Id == req.ID).FirstOrDefaultAsync();
                    if (model != null)
                    {


                        model.TransactionLevelId = req.TrasanctionLevel.ID;
                        model.Modified = DateTime.Now;
                        model.ModifiedBy = req.ModifiedBy;


                        TransactionJournal journal = new TransactionJournal();
                        journal.TransactionId = model.Id;
                        journal.BuyerId = model.BuyerId;
                        journal.SellerId = model.SellerId;
                        journal.NegoBarangId = model.NegoBarangId;
                        journal.TransactionLevelId = model.TransactionLevelId;
                        journal.Created = model.Created;
                        journal.CreatedBy = model.CreatedBy;
                        journal.RowStatus = true;

                        await db.TransactionJournal.AddAsync(journal);
                        await db.SaveChangesAsync();
                        response.Message = "Data has been Saved";

                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = "Data Not Found";
                    }
                }
                catch (Exception ex)
                {

                    response.Message = ex.ToString();
                    response.IsSuccess = false;
                }

            }
            return response;
        }

        public async Task<TransactionResponse> CancelTransaction(TransactionRequest req)
        {
            TransactionResponse response = new TransactionResponse();

            if (db != null)
            {
                try
                {



                    Transaction model = await db.Transaction.Where(x => x.RowStatus == true && x.Id == req.ID).FirstOrDefaultAsync();
                    if (model != null)
                    {
                        var query = db.TransactionLevel.Where(x => x.RowStatus == true && model.TransactionLevelId == x.Id).FirstOrDefault();
                        var findLevel = await (from level in db.TransactionLevel
                                               join status in db.TransactionStatus
                                               on level.TransactionStatusId equals status.Id
                                               where status.Name.Contains("Cancel") && status.RowStatus == true && level.RowStatus == true
                                               && level.Name == query.Name
                                               select level).FirstOrDefaultAsync();



                        model.TransactionLevelId = findLevel.Id;
                        model.Modified = DateTime.Now;
                        model.ModifiedBy = req.ModifiedBy;

                        TransactionJournal journal = new TransactionJournal();
                        journal.TransactionId = model.Id;
                        journal.BuyerId = model.BuyerId;
                        journal.SellerId = model.SellerId;
                        journal.NegoBarangId = model.NegoBarangId;
                        journal.TransactionLevelId = model.TransactionLevelId;
                        journal.Created = model.Created;
                        journal.CreatedBy = model.CreatedBy;
                        journal.RowStatus = true;

                        await db.TransactionJournal.AddAsync(journal);
                        await db.SaveChangesAsync();
                        response.Message = "Data has been Saved";

                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = "Data Not Found";
                    }
                }
                catch (Exception ex)
                {

                    response.Message = ex.ToString();
                    response.IsSuccess = false;
                }

            }
            return response;
        }

        public async Task<TransactionResponse> DeletePost(TransactionRequest req)
        {
            TransactionResponse response = new TransactionResponse();

            if (db != null)
            {
                try
                {
                    Transaction model = await db.Transaction.Where(x => x.RowStatus == true && x.Id == req.ID).FirstOrDefaultAsync();
                    if (model != null)
                    {
                        model.RowStatus = false;
                        await db.SaveChangesAsync();

                        response.Message = "Data has been Saved";
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = "Data Not Found";
                    }
                }
                catch (Exception ex)
                {

                    response.Message = ex.ToString();
                    response.IsSuccess = false;
                }

            }
            return response;
        }

        public async Task<TransactionResponse> GetAll(string search, string order, string orderDir, int startRec, int pageSize, int draw)
        {
            TransactionResponse response = new TransactionResponse();

            if (db != null)
            {
                try
                {

                    response.ListTransaction = await (from transaction in db.Transaction
                                                      join buyer in db.UserProfile
                                                      on transaction.BuyerId equals buyer.Id
                                                      join seller in db.UserProfile
                                                      on transaction.SellerId equals seller.Id
                                                      join negoBarang in db.NegoBarang
                                                      on transaction.NegoBarangId equals negoBarang.Id
                                                      join level in db.TransactionLevel
                                                      on transaction.TransactionLevelId equals level.Id
                                                      join step in db.TransactionStep
                                                      on level.TransactionStepId equals step.Id
                                                      join status in db.TransactionStatus
                                                      on level.TransactionStatusId equals status.Id
                                                      join barang in db.Barang
                                                      on negoBarang.BarangId equals barang.Id
                                                      join warna in db.Warna
                                                      on barang.WarnaId equals warna.Id
                                                      join userNegoBarang in db.UserProfile
                                                      on negoBarang.UserProfileId equals userNegoBarang.Id
                                                      join userDetail in db.UserDetail
                                                      on seller.Id equals userDetail.UserProfileId
                                                      join dealer in db.Dealer
                                                      on userDetail.KodeDealer equals dealer.Kode
                                                      join tBarang in db.TypeBarang
                                                      on barang.TypeBarangId equals tBarang.Id
                                                      join mdlBarang in db.ModelBarang
                                                      on tBarang.ModelBarangId equals mdlBarang.Id
                                                      join merkBarang in db.Merk
                                                      on mdlBarang.MerkId equals merkBarang.Id
                                                      join kota in db.Kota
                                                      on dealer.KotaId equals kota.Id
                                                      where transaction.RowStatus == true
                                                      && barang.RowStatus == true
                                                      && buyer.RowStatus == true
                                                      && seller.RowStatus == true
                                                      && negoBarang.RowStatus == true
                                                      && level.RowStatus == true
                                                      && step.RowStatus == true
                                                      && status.RowStatus == true
                                                      && level.RowStatus == true
                                                      && warna.RowStatus == true
                                                      && userNegoBarang.RowStatus == true

                                                      && dealer.RowStatus == true
                                                      select new TransactionViewModel
                                                      {
                                                          ID = transaction.Id,
                                                          IDPembeli = buyer.Id,
                                                          NamaPembeli = buyer.Nama,
                                                          IDPenjual = seller.Id,
                                                          NamaPenjual = seller.Nama,
                                                          NamaStatus = status.Name,
                                                          NamaLangkah = step.Name,
                                                          HargaOTR = barang.HargaOtr,
                                                          HargaNego = negoBarang.Harga,
                                                          TipePenawaran = negoBarang.TypePenawaran,
                                                          TelpPembeli = buyer.Telp,
                                                          TelpPenjual = seller.Telp,
                                                          EmailPembeli = buyer.Email,
                                                          EmailPenjual = seller.Email,

                                                          NamaBarang = (merkBarang.Name + " " + mdlBarang.Name + " " + tBarang.Name + " " + warna.Name),
                                                          NamaDealerKota = dealer.Name + "-" + kota.Name,
                                                          NamaDealer = dealer.Name,
                                                          AlamatDealer = dealer.Alamat,
                                                          TelpDealer = dealer.Telepon,
                                                          KotaDealer = kota.Name,
                                                          KodeDealer = dealer.Kode,
                                                          Created = transaction.Created,
                                                          CreatedBy = transaction.CreatedBy,
                                                          Modified = transaction.Modified,
                                                          ModifiedBy = transaction.ModifiedBy

                                                      }).ToListAsync();
                    int totalRecords = response.ListTransaction.Count;
                    if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                    {
                        response.ListTransaction = response.ListTransaction.Where(p => p.NamaStatus.ToString().ToLower().Contains(search.ToLower()) ||
                                    p.NamaBarang.ToLower().Contains(search.ToLower()) ||
                                    p.NamaPembeli.ToLower().Contains(search.ToLower()) ||
                                    p.NamaPenjual.ToLower().Contains(search.ToLower()) ||
                                    p.NamaDealer.ToLower().Contains(search.ToLower()) ||

                                    p.NamaStatus.ToLower().Contains(search.ToLower())).ToList();


                    }

                    //response = SortByColumnWithOrder(order, orderDir, response);

                    int recFilter = response.ListTransaction.Count;

                    response.ListTransaction = response.ListTransaction.Skip(startRec).Take(pageSize).ToList();

                    response.draw = Convert.ToInt32(draw);
                    response.recordsTotal = totalRecords;
                    response.recordsFiltered = recFilter;
                    response.Message = "Success";
                }
                catch (Exception ex)
                {

                    response.Message = ex.ToString();
                    response.IsSuccess = false;
                }

            }
            return response;
        }

        public async Task<TransactionResponse> GetPost(TransactionRequest req)
        {
            TransactionResponse response = new TransactionResponse();

            if (db != null)
            {
                try
                {
                    response.ListTransaction = await (from transaction in db.Transaction
                                                      join buyer in db.UserProfile
                                                      on transaction.BuyerId equals buyer.Id
                                                      join seller in db.UserProfile
                                                      on transaction.SellerId equals seller.Id
                                                      join negoBarang in db.NegoBarang
                                                      on transaction.NegoBarangId equals negoBarang.Id
                                                      join level in db.TransactionLevel
                                                      on transaction.TransactionLevelId equals level.Id
                                                      join step in db.TransactionStep
                                                      on level.TransactionStepId equals step.Id
                                                      join status in db.TransactionStatus
                                                      on level.TransactionStatusId equals status.Id
                                                      join barang in db.Barang
                                                      on negoBarang.BarangId equals barang.Id
                                                      join warna in db.Warna
                                                      on barang.WarnaId equals warna.Id
                                                      join userNegoBarang in db.UserProfile
                                                      on negoBarang.UserProfileId equals userNegoBarang.Id
                                                      join userDetail in db.UserDetail
                                                      on seller.Id equals userDetail.UserProfileId
                                                      join dealer in db.Dealer
                                                      on userDetail.KodeDealer equals dealer.Kode
                                                      join tBarang in db.TypeBarang
                                                      on barang.TypeBarangId equals tBarang.Id
                                                      join mdlBarang in db.ModelBarang
                                                      on tBarang.ModelBarangId equals mdlBarang.Id
                                                      join merkBarang in db.Merk
                                                      on mdlBarang.MerkId equals merkBarang.Id
                                                      join kota in db.Kota
                                                      on dealer.KotaId equals kota.Id
                                                      where transaction.RowStatus == true
                                                      && barang.RowStatus == true
                                                      && buyer.RowStatus == true
                                                      && seller.RowStatus == true
                                                      && negoBarang.RowStatus == true
                                                      && level.RowStatus == true
                                                      && step.RowStatus == true
                                                      && status.RowStatus == true
                                                      && level.RowStatus == true
                                                      && warna.RowStatus == true
                                                      && userNegoBarang.RowStatus == true

                                                      && dealer.RowStatus == true
                                                      where transaction.Id == req.ID
                                                      select new TransactionViewModel
                                                      {
                                                          ID = transaction.Id,
                                                          IDPembeli = buyer.Id,
                                                          NamaPembeli = buyer.Nama,
                                                          IDPenjual = seller.Id,
                                                          NamaPenjual = seller.Nama,
                                                          NamaStatus = status.Name,
                                                          NamaLangkah = step.Name,
                                                          HargaOTR = barang.HargaOtr,
                                                          HargaNego = negoBarang.Harga,
                                                          //NegoBarang = new NegoBarangViewModel
                                                          //{
                                                          //    ID = negoBarang.Id,
                                                          //    userProfieViewModel = new UserProfileViewModel
                                                          //    {
                                                          //        ID = userNegoBarang.Id,
                                                          //        Name = userNegoBarang.Name,
                                                          //        Email = userNegoBarang.Email,
                                                          //        Telp = userNegoBarang.Telp,
                                                          //        Facebook = userNegoBarang.Facebook,
                                                          //        IG = userNegoBarang.Ig,

                                                          //        JenisKelamin = userNegoBarang.JenisKelamin

                                                          //    },
                                                          //    Barang = new BarangViewModel
                                                          //    {
                                                          //        Id = barang.Id,
                                                          //        HargaOtr = barang.HargaOtr,
                                                          //        Name = barang.Name,

                                                          //    },
                                                          //    Harga = negoBarang.Harga,


                                                          //},
                                                          TrasanctionLevel = new TransactionLevelViewModel
                                                          {
                                                              ID = level.Id,
                                                              Status = new TransactionStatusViewModel
                                                              {
                                                                  Name = status.Name
                                                              },
                                                              Step = new TransactionStepViewModel
                                                              {
                                                                  Name = step.Name
                                                              }

                                                          },
                                                          NamaBarang = (merkBarang.Name + " " + mdlBarang.Name + " " + tBarang.Name + " " + warna.Name),
                                                          NamaDealer = dealer.Name + "-" + kota.Name,
                                                          Created = transaction.Created,
                                                          CreatedBy = transaction.CreatedBy,
                                                          Modified = transaction.Modified,
                                                          ModifiedBy = transaction.ModifiedBy
                                                      }).ToListAsync();
                    response.Message = "Success";
                }
                catch (Exception ex)
                {

                    response.Message = ex.ToString();
                    response.IsSuccess = false;
                }

            }
            return response;
        }

        public async Task<TransactionResponse> UpdatePost(TransactionRequest req)
        {
            TransactionResponse response = new TransactionResponse();

            if (db != null)
            {
                try
                {
                    Transaction model = await db.Transaction.Where(x => x.RowStatus == true && x.Id == req.ID).FirstOrDefaultAsync();
                    if (model != null)
                    {

                        model.BuyerId = req.Buyer.ID;
                        model.SellerId = req.Seller.ID;
                        model.NegoBarangId = req.NegoBarang.ID;
                        model.TransactionLevelId = req.TrasanctionLevel.ID;
                        model.Modified = DateTime.Now;
                        model.ModifiedBy = model.ModifiedBy;
                        await db.SaveChangesAsync();

                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = "Data Not Found";
                    }
                }
                catch (Exception ex)
                {

                    response.Message = ex.ToString();
                    response.IsSuccess = false;
                }

            }
            return response;
        }

        public static TransactionResponse SortByColumnWithOrder(string order, string orderDir, TransactionResponse response)
        {
            try
            {
                #region Sorting Salesman  
                //switch (order)
                //{
                //case "0":
                //    // Setting.    
                //    response.ListTransaction = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? response.ListTransaction.OrderByDescending(p => p.Name).ToList()
                //                                         : response.ListTransaction.OrderBy(p => p.Name).ToList();
                //    break;
                //case "1":
                //    // Setting.    
                //    response.ListTransaction = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? response.ListTransaction.OrderByDescending(p => p.Email).ToList()
                //                                         : response.ListTransaction.OrderBy(p => p.Email).ToList();
                //    break;

                //case "2":
                //    // Setting.    
                //    response.ListTransaction = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? response.ListTransaction.OrderByDescending(p => p.Telp).ToList()
                //                                         : response.ListTransaction.OrderBy(p => p.Telp).ToList();
                //    break;
                //case "3":
                //    // Setting.    
                //    response.ListTransaction = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? response.ListTransaction.OrderByDescending(p => p.VerifiedDate).ToList()
                //                                         : response.ListTransaction.OrderBy(p => p.VerifiedDate).ToList();
                //    break;
                //case "4":
                //    // Setting.    
                //    response.ListTransaction = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? response.ListTransaction.OrderByDescending(p => p.VerifiedBy).ToList()
                //                                         : response.ListTransaction.OrderBy(p => p.VerifiedBy).ToList();
                //    break;
                //default:
                //    // Setting.    
                //    response.ListTransaction = response.ListTransaction.OrderByDescending(p => p.ID).ToList();
                //    break;
                //}
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
                //log.Error("UserFacade.SortByColumnWithOrder :" + ex.ToString());
            }
            return response;
        }

        public async Task<TransactionResponse> GetJournalByTransaction(TransactionRequest req)
        {
            TransactionResponse response = new TransactionResponse();

            if (db != null)
            {
                try
                {
                    response.ListJournalByTransaction = await (from journal in db.TransactionJournal
                                                               join level in db.TransactionLevel
                                                               on journal.TransactionLevelId equals level.Id
                                                               join status in db.TransactionStatus
                                                               on level.TransactionStatusId equals status.Id
                                                               join step in db.TransactionStep
                                                               on level.TransactionStepId equals step.Id
                                                               where journal.RowStatus == true && journal.TransactionId == req.ID
                                                               && level.RowStatus == true
                                                               && status.RowStatus == true
                                                               && step.RowStatus == true
                                                               select new TransactionJournalViewModel
                                                               {
                                                                   TransactionStatusName = status.Name,
                                                                   Created = journal.Created,
                                                                   CreatedBy = journal.CreatedBy,
                                                                   TransactionStepName = step.Name
                                                               }).ToListAsync();
                    response.Message = "Success";
                }
                catch (Exception ex)
                {

                    response.Message = ex.ToString();
                    response.IsSuccess = false;
                }

            }
            return response;
        }

        public async Task<List<TransactionStatus>> GetAllStatus()
        {
            List<TransactionStatus> result = new List<TransactionStatus>();

            try
            {
                result = await db.TransactionStatus.Where(x => x.RowStatus == true).ToListAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return result;
        }

        public async Task<List<SP_ReportByStatusID>> GetReportByStatusID(long id, string endDate)
        {
            try
            {
                return await db.Set<SP_ReportByStatusID>().FromSql("EXEC sp_ReportByStatusID {0},{1}", id, endDate).AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<SP_GetAllBidByUserProfileID>> GetAllBidByUserProfileID(long UserProfileID)
        {

            try
            {
                return await db.Set<SP_GetAllBidByUserProfileID>().FromSql("EXEC sp_GetAllBidByUserProfileID {0}", UserProfileID).AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<List<SP_GetAllAskByUserProfileID>> GetAllAskByUserProfileID(long UserProfileID)
        {
            try
            {
                return await db.Set<SP_GetAllAskByUserProfileID>().FromSql("EXEC sp_GetAllAskByUserProfileID {0}", UserProfileID).AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<List<sp_GetAllBidAndBuyByUserProfileID>> GetAllBidAndBuyByUserProfileID(long UserProfileID)
        {
            try
            {
                return await db.Set<sp_GetAllBidAndBuyByUserProfileID>().FromSql("EXEC sp_GetAllBidAndBuyByUserProfileID {0}", UserProfileID).AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<UserProfile> GetUserProfile(string UserName)
        {

            try
            {
                return await db.UserProfile.Where(x => x.RowStatus == true && x.Email == UserName).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
