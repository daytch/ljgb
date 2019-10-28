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
                        var query =  db.TransactionLevel.Where(x => x.RowStatus == true && model.TransactionLevelId == x.Id).FirstOrDefault();
                        var findLevel = await  (from level in db.TransactionLevel
                                         join status in db.TransactionStatus
                                         on level.TransactionStatusId equals status.Id
                                         where status.Name.Contains("Cancel") && status.RowStatus == true && level.RowStatus == true
                                         && level.Name == query.Name
                                         select level).FirstOrDefaultAsync();



                        model.TransactionLevelId = findLevel.Id;
                        model.Modified = DateTime.Now;
                        model.ModifiedBy = req.ModifiedBy;
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

        public async Task<TransactionResponse> GetAll()
        {
            TransactionResponse response = new TransactionResponse();

            if (db != null)
            {
                try
                {
                    response.ListTransaction = await  (from transaction in db.Transaction
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
                                                       select new TransactionViewModel
                                                       {
                                                           ID = transaction.Id,
                                                           Buyer = new UserProfileViewModel
                                                           {
                                                               ID = buyer.Id,
                                                               Name = buyer.Nama,
                                                               Email = buyer.Email,
                                                               Telp = buyer.Telp,
                                                               Facebook = buyer.Facebook,
                                                               IG = buyer.Ig,
                                                               JenisKelamin = buyer.JenisKelamin
                                                           },
                                                           Seller = new UserProfileViewModel
                                                           {
                                                               ID = seller.Id,
                                                               Name = seller.Nama,
                                                               Email = seller.Email,
                                                               Telp = seller.Telp,
                                                               Facebook = seller.Facebook,
                                                               IG = seller.Ig,
                                                               JenisKelamin = seller.JenisKelamin
                                                           },
                                                           NegoBarang = new NegoBarangViewModel
                                                           {
                                                               ID = negoBarang.Id,
                                                               userProfieViewModel = new UserProfileViewModel
                                                               {
                                                                   ID = userNegoBarang.Id,
                                                                   Name = userNegoBarang.Nama,
                                                                   Email = userNegoBarang.Email,
                                                                   Telp = userNegoBarang.Telp,
                                                                   Facebook = userNegoBarang.Facebook,
                                                                   IG = userNegoBarang.Ig,
                                                                   JenisKelamin = userNegoBarang.JenisKelamin

                                                               },
                                                               Barang = new BarangViewModel
                                                               {
                                                                   Id = barang.Id,
                                                                   HargaOtr = barang.HargaOtr,
                                                                   Name = barang.Name,
                                                               },
                                                               Harga = negoBarang.Harga,


                                                           },
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

        public async Task<TransactionResponse> GetPost(TransactionRequest req)
        {
            TransactionResponse response = new TransactionResponse();

            if (db != null)
            {
                try
                {
                    response.ListTransaction = await(from transaction in db.Transaction
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
                                                      && transaction.Id == req.ID
                                                      select new TransactionViewModel
                                                      {
                                                          ID = transaction.Id,
                                                          Buyer = new UserProfileViewModel
                                                          {
                                                              ID = buyer.Id,
                                                              Name = buyer.Nama,
                                                              Email = buyer.Email,
                                                              Telp = buyer.Telp,
                                                              Facebook = buyer.Facebook,
                                                              IG = buyer.Ig,
                                                              JenisKelamin = buyer.JenisKelamin
                                                          },
                                                          Seller = new UserProfileViewModel
                                                          {
                                                              ID = seller.Id,
                                                              Name = seller.Nama,
                                                              Email = seller.Email,
                                                              Telp = seller.Telp,
                                                              Facebook = seller.Facebook,
                                                              IG = seller.Ig,
                                                              JenisKelamin = seller.JenisKelamin
                                                          },
                                                          NegoBarang = new NegoBarangViewModel
                                                          {
                                                              ID = negoBarang.Id,
                                                              userProfieViewModel = new UserProfileViewModel
                                                              {
                                                                  ID = userNegoBarang.Id,
                                                                  Name = userNegoBarang.Nama,
                                                                  Email = userNegoBarang.Email,
                                                                  Telp = userNegoBarang.Telp,
                                                                  Facebook = userNegoBarang.Facebook,
                                                                  IG = userNegoBarang.Ig,
                                                                  JenisKelamin = userNegoBarang.JenisKelamin

                                                              },
                                                              Barang = new BarangViewModel
                                                              {
                                                                  Id = barang.Id,
                                                                  HargaOtr = barang.HargaOtr,
                                                                  Name = barang.Name,
                                                              },
                                                              Harga = negoBarang.Harga,
                                                              
                                                              
                                                          },
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
    }
}
