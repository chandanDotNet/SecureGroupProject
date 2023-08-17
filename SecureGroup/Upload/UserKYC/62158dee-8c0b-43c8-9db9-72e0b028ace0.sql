USE [SecureGroup]
GO
/****** Object:  Table [dbo].[Attendance]    Script Date: 25-07-2023 14:09:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Attendance](
	[AttendanceId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[Date] [date] NULL,
	[LoginTime] [time](0) NULL,
	[LogoutTime] [time](0) NULL,
	[DurationTime] [time](0) NULL,
	[Reason] [nvarchar](max) NULL,
	[AttendanceStatusId] [int] NULL,
	[DeviationsLoginTime] [time](0) NULL,
	[DeviationsLogoutTime] [time](0) NULL,
	[DeviationsDurationTime] [time](0) NULL,
	[DeviationsApprovalStatusId] [int] NULL,
	[AttendanceApproveRejectBy] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[UpdateDate] [datetime] NULL,
	[IsActive] [bit] NULL,
 CONSTRAINT [PK_Attendance] PRIMARY KEY CLUSTERED 
(
	[AttendanceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LeaveApply]    Script Date: 25-07-2023 14:09:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LeaveApply](
	[LeaveId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[LeaveReason] [nvarchar](max) NULL,
	[LeaveType] [int] NULL,
	[Document] [nvarchar](max) NULL,
	[LeaveStatus] [int] NULL,
	[LeaveApproveRejectBy] [int] NULL,
	[LeaveRejectedReason] [nvarchar](max) NULL,
	[NumberOfleaveCount] [nvarchar](100) NULL,
	[IsActive] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LeaveManagement]    Script Date: 25-07-2023 14:09:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LeaveManagement](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[TotalLeave] [decimal](5, 2) NULL,
	[BalanceLeave] [decimal](5, 2) NULL,
	[TakenLeave] [decimal](5, 2) NULL,
	[IsActive] [bit] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LogManagement]    Script Date: 25-07-2023 14:09:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LogManagement](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[RoleId] [int] NULL,
	[IpAddress] [nvarchar](50) NULL,
	[MacAddress] [nvarchar](50) NULL,
	[Browser] [nvarchar](50) NULL,
	[LogDateTime] [datetime] NULL,
	[IsLogin] [bit] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Reimbursement]    Script Date: 25-07-2023 14:09:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Reimbursement](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ExpenseFor] [nvarchar](max) NULL,
	[TotalExpenseAmount] [nvarchar](50) NULL,
	[ClaimAmount] [nvarchar](50) NULL,
	[ApprovedAmount] [nvarchar](50) NULL,
	[ExpenseDate] [datetime] NULL,
	[ExpenseToDate] [datetime] NULL,
	[ExpenseDocument] [nvarchar](50) NULL,
	[Signature] [nvarchar](50) NULL,
	[ApprovedBy] [int] NULL,
	[ApprovedDate] [datetime] NULL,
	[StatusId] [int] NULL,
	[ClaimBy] [int] NULL,
	[Comments] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdateDate] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[IsActive] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReimbursementDocument]    Script Date: 25-07-2023 14:09:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReimbursementDocument](
	[DocumentId] [int] IDENTITY(1,1) NOT NULL,
	[ReimbursementId] [int] NULL,
	[DocumentName] [nvarchar](max) NULL,
	[UploadedBy] [int] NULL,
	[UploadedDate] [datetime] NULL,
	[IsActive] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
