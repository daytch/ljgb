USE [ljgb]
GO

/****** Object:  StoredProcedure [dbo].[sp_GetAllAskByUserProfileID]    Script Date: 11/23/2019 5:30:17 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





-- =============================================
-- Author:		victor
-- Create date: 05-Nov-2019
-- Description:	Get All ASK By UserProfileID
-- =============================================
ALTER PROCEDURE [dbo].[sp_GetAllAskByUserProfileID] 
	-- Add the parameters for the stored procedure here
	@UserProfileID bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	

	SELECT 
		nb.ID as id,
		mrk.Name as merkName, 
		mb.Name as modelName, 
		tb.Name as typeName, 
		wrn.Name as colour, 
		nb.TypePenawaran as typePenawaran, 
		nb.Harga as price
	FROM NegoBarang nb
		JOIN UserProfile usr on nb.UserProfileID = usr.ID
		JOIN Barang brg  on nb.BarangID = brg.ID
		JOIN TypeBarang tb on brg.TypeBarangID = tb.ID
		JOIN ModelBarang mb on tb.ModelBarangID = mb.ID
		JOIN Merk mrk on mb.MerkID = mrk.ID
		JOIN Warna wrn on brg.WarnaID = wrn.ID
	WHERE nb.RowStatus = 1 
		AND usr.RowStatus = 1 
		AND nb.TypePenawaran = 'ask'
		AND usr.ID = @UserProfileID
		
 
END



GO


