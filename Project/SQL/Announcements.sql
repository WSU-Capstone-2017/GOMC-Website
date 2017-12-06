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

ALTER TABLE [dbo].[Announcements] DROP CONSTRAINT [FK_Announcements_AuthorId]
GO

/****** Object:  Table [dbo].[Announcements]    Script Date: 12/5/2017 8:44:30 PM ******/
DROP TABLE [dbo].[Announcements]
GO

/****** Object:  Table [dbo].[Announcements]    Script Date: 12/5/2017 8:44:30 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Announcements](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AuthorId] [int] NOT NULL,
	[Content] [nvarchar](max) NOT NULL,
	[Created] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Announcements]  WITH CHECK ADD  CONSTRAINT [FK_Announcements_AuthorId] FOREIGN KEY([AuthorId])
REFERENCES [dbo].[Logins] ([Id])
GO

ALTER TABLE [dbo].[Announcements] CHECK CONSTRAINT [FK_Announcements_AuthorId]
GO


