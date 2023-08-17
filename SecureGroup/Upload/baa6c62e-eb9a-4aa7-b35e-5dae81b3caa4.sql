USE [SecureGroup]
GO

/****** Object:  Table [dbo].[User]    Script Date: 06-06-2023 13:50:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WareHouse](
	[WareHouseId] [int] identity(1,1) NOT NULL,
	[WareHouseName] [nvarchar](50) NULL,
	[Address] [nvarchar](50) NULL,
	[StateId] int NULL,
	[CityId] int NULL,
	[Block] [nvarchar](50) NULL,
	[ZipCode] [nvarchar](20) NULL,
	CountryId int null,
	WareHouseAdminId int null,
	[IsActive] [bit] NULL
)

CREATE TABLE [dbo].[WareHouseAdminDetails](
    [WareHouseAdminId] [int] identity(1,1) NOT NULL,
	[WareHouseId] [int] NOT NULL,
	[UserId] [int] NOT NULL,	
	[UserName] [nvarchar](50) NULL,
	[Email] [nvarchar](50) NULL,
	[ContactNumber] [nvarchar](50) NULL,
	[Status] [int]  NULL,
	[StartDate] datetime null,
	[EndDate] datetime null,
	[IsActive] [bit] NULL
)

CREATE TABLE [dbo].[WareHouseStockManagement](
    [Id] [int]identity(1,1) NOT NULL,
	[WareHouseId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[LiveProductQuantity] bigint NULL,
	[QuantityOnHand] bigint NULL,
	[QuantityOnOrder] bigint NULL,
	[QuantityAllocated] bigint NULL,
	[ProductUnitId] [int] NULL,
	[Status] [int]  NULL,	
	CreatedBy bigint null,
	CreatedDate datetime null,
	UpdatedBy bigint null,
	LastUpdateDate datetime null,
	[IsActive] [bit] NULL
)

CREATE TABLE [dbo].[WareHouseProductStockManagement](
    [Id] [int]identity(1,1) NOT NULL,
	[SourceWareHouseId] [int] NOT NULL,
	[DestinationWareHouseId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[ProductQuantity] [nvarchar](50) NULL,
	[QuantityType] [nvarchar](50) NULL,
	[TransferDate] [datetime] NULL,
	[TransferBy] [int]  NULL,	
	[Status] [int]  NULL,	
	CreatedBy bigint null,
	CreatedDate datetime null,
	UpdatedBy bigint null,
	LastUpdateDate datetime null,
	[IsActive] [bit] NULL
)

CREATE TABLE [dbo].[Product](
    [ProductId] [int]identity(1,1) NOT NULL,	
	[ProductName] [nvarchar](100) NULL,	
	[CategoryId] [int] NOT NULL,
	[Status] [int]  NULL,	
	CreatedBy bigint null,
	CreatedDate datetime null,
	UpdatedBy bigint null,
	LastUpdateDate datetime null,
	[IsActive] [bit] NULL
)

CREATE TABLE [dbo].[ProductUnit](
    [UnitId] [int]identity(1,1) NOT NULL,	
	[UnitName] [nvarchar](100) NULL,	
	[IsActive] [bit] NULL
)

CREATE TABLE [dbo].[ProductCategory](
    [CategoryId] [int]identity(1,1) NOT NULL,	
	[CategoryName] [nvarchar](100) NULL,	
	[IsActive] [bit] NULL
)








