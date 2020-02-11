
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


create table AccountTypes (
	AccountTypeId int not null,
	Description nvarchar(max) null,
	primary key (AccountTypeId)
)

create table Accounts (
	AccountId int not null,
	CreationDate date not null,
	AccountTypeId int not null,
	foreign key (AccountTypeId) references AccountTypes(AccountTypeId),
	primary key (AccountId)
)

create table AccountDetails (
	AccountDetailId int not null,
	AccountId int not null,
	Amount numeric(12,2) not null,
	primary key (AccountDetailId),
	foreign key (AccountId) references Accounts(AccountId)
)

insert into AccountTypes
values(1,'Good Account')

insert into Accounts
values(1, GETDATE(), 1)

insert into AccountDetails
values(1,1,100)


insert into AccountDetails
values(2,1,300)


insert into AccountDetails
values(3,1,500)