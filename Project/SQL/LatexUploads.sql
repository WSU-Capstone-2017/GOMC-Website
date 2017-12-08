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

ALTER TABLE [dbo].[LatexUploads] DROP CONSTRAINT [FK_LatexUploads_AuthorId]
GO

/****** Object:  Table [dbo].[LatexUploads]    Script Date: 12/5/2017 8:46:29 PM ******/
DROP TABLE [dbo].[LatexUploads]
GO

/****** Object:  Table [dbo].[LatexUploads]    Script Date: 12/5/2017 8:46:29 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LatexUploads](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AuthorId] [int] NOT NULL,
	[Version] [nvarchar](max) NOT NULL,
	[HtmlZip] [varbinary](max) NOT NULL,
	[Pdf] [varbinary](max) NOT NULL,
	[Created] [datetime] NOT NULL,
	[LatexFile] [nvarchar](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[LatexUploads]  WITH CHECK ADD  CONSTRAINT [FK_LatexUploads_AuthorId] FOREIGN KEY([AuthorId])
REFERENCES [dbo].[Logins] ([Id])
GO

ALTER TABLE [dbo].[LatexUploads] CHECK CONSTRAINT [FK_LatexUploads_AuthorId]
GO


