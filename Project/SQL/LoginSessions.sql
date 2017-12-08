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

ALTER TABLE [dbo].[LoginSessions] DROP CONSTRAINT [FK_LoginSessions_LoginId]
GO

/****** Object:  Table [dbo].[LoginSessions]    Script Date: 12/5/2017 8:47:04 PM ******/
DROP TABLE [dbo].[LoginSessions]
GO

/****** Object:  Table [dbo].[LoginSessions]    Script Date: 12/5/2017 8:47:04 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LoginSessions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LoginId] [int] NOT NULL,
	[Session] [uniqueidentifier] NOT NULL,
	[Expiration] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_LoginSessions_Session] UNIQUE NONCLUSTERED 
(
	[Session] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[LoginSessions]  WITH CHECK ADD  CONSTRAINT [FK_LoginSessions_LoginId] FOREIGN KEY([LoginId])
REFERENCES [dbo].[Logins] ([Id])
GO

ALTER TABLE [dbo].[LoginSessions] CHECK CONSTRAINT [FK_LoginSessions_LoginId]
GO


