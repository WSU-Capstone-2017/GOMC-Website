/*    ==Scripting Parameters==

    Source Server Version : SQL Server 2016 (13.0.4422)
    Source Database Engine Edition : Microsoft SQL Server Express Edition
    Source Database Engine Type : Standalone SQL Server

    Target Server Version : SQL Server 2017
    Target Database Engine Edition : Microsoft SQL Server Standard Edition
    Target Database Engine Type : Standalone SQL Server
*/

USE [projectdb]
GO
/****** Object:  StoredProcedure [dbo].[GetLoginIdFromSession]    Script Date: 12/8/2017 6:59:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ahmed Taher
-- Create date: 11/2/2017
-- Description:	The purpose of this procedure is to extract an 
-- Id of UserLoginModels when given a session value.
-- =============================================
ALTER PROCEDURE [dbo].[GetLoginIdFromSession] 
	-- Add the parameters for the stored procedure here
	@SessionInput uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT TOP 1 * FROM dbo.LoginSessions
	WHERE Session = @SessionInput
END
