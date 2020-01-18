
USE [Dapper.Json]
GO
/****** Object:  Table [dbo].[Children]    Script Date: 1/18/2020 2:36:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Children](
	[ChildId] [int] NOT NULL,
	[Description] [varchar](255) NULL,
	[ParentId] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Parents]    Script Date: 1/18/2020 2:36:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Parents](
	[ParentId] [int] NOT NULL,
	[Description] [varchar](255) NULL,
	[SiblingId] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Siblings]    Script Date: 1/18/2020 2:36:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Siblings](
	[SiblingId] [int] NOT NULL,
	[Description] [varchar](255) NULL
) ON [PRIMARY]
GO
INSERT [dbo].[Children] ([ChildId], [Description], [ParentId]) VALUES (1, N'Children Test', 1)
GO
INSERT [dbo].[Parents] ([ParentId], [Description], [SiblingId]) VALUES (1, N'Parent Test', 1)
GO
INSERT [dbo].[Siblings] ([SiblingId], [Description]) VALUES (1, N'Sibling Test')
GO
