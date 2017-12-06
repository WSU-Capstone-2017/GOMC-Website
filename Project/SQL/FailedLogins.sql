/*    ==Scripting Parameters==

    Source Server Version : SQL Server 2016 (13.0.4422)
    Source Database Engine Edition : Microsoft SQL Server Express Edition
    Source Database Engine Type : Standalone SQL Server

    Target Server Version : SQL Server 2016
    Target Database Engine Edition : Microsoft SQL Server Express Edition
    Target Database Engine Type : Standalone SQL Server
*/

USE [projectdb]
GO

ALTER TABLE [dbo].[FailedLogins] DROP CONSTRAINT [FK_FailedLogins_LoginId]
GO

/****** Object:  Table [dbo].[FailedLogins]    Script Date: 12/5/2017 8:46:10 PM ******/
DROP TABLE [dbo].[FailedLogins]
GO

/****** Object:  Table [dbo].[FailedLogins]    Script Date: 12/5/2017 8:46:10 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FailedLogins](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LoginId] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[FailedLogins]  WITH CHECK ADD  CONSTRAINT [FK_FailedLogins_LoginId] FOREIGN KEY([LoginId])
REFERENCES [dbo].[Logins] ([Id])
GO

ALTER TABLE [dbo].[FailedLogins] CHECK CONSTRAINT [FK_FailedLogins_LoginId]
GO


