USE [ActorDirectorDB]
GO

/****** Object:  Table [dbo].[MemberProfile]    Script Date: 14-Oct-17 9:37:21 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MemberProfile](
	[Id] [bigint] NULL,
	[FirstName] [varchar](20) NULL,
	[LastName] [varchar](20) NULL,
	[DOB] [datetime] NULL,
	[HighestEducation] [varchar](20) NULL,
	[HomeDistrict] [varchar](20) NULL,
	[Experience] [tinyint] NULL,
	[MobilePhone] [varchar](20) NULL,
	[FK_FROM_IdentityUser] [bigint] NULL
) ON [PRIMARY]
GO


