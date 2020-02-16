USE [ljgb]
GO

/****** Object:  StoredProcedure [dbo].[sp_GetListNormal]    Script Date: 2/12/2020 10:22:35 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



ALTER PROCEDURE [dbo].[sp_GetListNormal]   
 -- Add the parameters for the stored procedure here  
 @Kota varchar(50),  
 @Total int  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
    -- Insert statements for procedure here  
	if @Kota is null or @Kota = ''
		BEGIN
			 IF EXISTS (
					SELECT MIN(b.id)
					FROM   barang b  
					LEFT OUTER JOIN negobarang nb ON b.id = nb.barangid 
					JOIN (  
							SELECT max(ID) AS ID,BarangID,MIN(harga) AS harga
							from NegoBarang group by BarangID  
							) AS maxNB on nb.ID=maxNB.ID
					LEFT OUTER JOIN userprofile up ON nb.userprofileid = up.id   
					LEFT OUTER JOIN kota k ON up.kotaid = k.id WHERE b.Category='other' AND b.RowStatus=1 AND nb.RowStatus=1 AND up.RowStatus=1 AND k.RowStatus=1
					GROUP BY b.TypeBarangID
					)
			 BEGIN				
				SELECT MIN(b.id) as ID, MIN(b.Name) as Name,MAX(b.year) as itemYear, MIN(b.photopath) as image, MAX(nb.harga) as price
				FROM   barang b  
				LEFT OUTER JOIN negobarang nb ON b.id = nb.barangid 
				JOIN (  
						SELECT max(ID) AS ID,BarangID,MIN(harga) AS harga
						from NegoBarang group by BarangID  
						) AS maxNB on nb.ID=maxNB.ID
				LEFT OUTER JOIN userprofile up ON nb.userprofileid = up.id   
				LEFT OUTER JOIN kota k ON up.kotaid = k.id WHERE b.Category='other' AND b.RowStatus=1 AND nb.RowStatus=1 AND up.RowStatus=1 AND k.RowStatus=1
				GROUP BY b.TypeBarangID
			 END
			 ELSE
			 BEGIN
				 SELECT TOP(@Total) MIN(b.id) as ID, MIN(b.Name) as Name,MAX(b.year) as itemYear, MIN(b.photopath) as image, MAX(nb.harga) as price
				 FROM   barang b   
					 LEFT OUTER JOIN negobarang nb   
					  ON b.id = nb.barangid   
					 JOIN (  
					SELECT max(ID) AS ID,BarangID,MIN(harga) AS harga   
					from NegoBarang group by BarangID  
					 ) AS maxNB on nb.ID=maxNB.ID  
					 LEFT OUTER JOIN userprofile up   
					  ON nb.userprofileid = up.id   
					 LEFT OUTER JOIN kota k   
					  ON up.kotaid = k.id WHERE b.RowStatus=1 AND nb.RowStatus=1 AND up.RowStatus=1 AND k.RowStatus=1
					  GROUP BY b.TypeBarangID 
			  END
		END
	ELSE
		BEGIN
			SELECT TOP(@Total) MIN(b.id) as ID, MIN(b.Name) as Name,MAX(b.year) as itemYear, MIN(b.photopath) as image, MAX(nb.harga) as price
			 FROM   barang b   
				 LEFT OUTER JOIN negobarang nb   
				  ON b.id = nb.barangid   
				 JOIN (  
				SELECT max(ID) AS ID,BarangID,MIN(harga) AS harga   
				from NegoBarang group by BarangID  
				 ) AS maxNB on nb.ID=maxNB.ID  
				 LEFT OUTER JOIN userprofile up   
				  ON nb.userprofileid = up.id   
				 LEFT OUTER JOIN kota k   
				  ON up.kotaid = k.id   
			 WHERE  k.Name = @Kota --LIKE '%%'   
				 -- AND nb.typepenawaran = 'ask' 
				  GROUP BY b.TypeBarangID   
		END

 -- ORDER  BY b.id DESC   
END  
  
  
GO


