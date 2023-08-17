USE [SecureGroup]
GO
/****** Object:  StoredProcedure [dbo].[uspUserDetails]    Script Date: 25-07-2023 14:04:28 ******/
DROP PROCEDURE [dbo].[uspUserDetails]
GO
/****** Object:  StoredProcedure [dbo].[SP_WHStockManagement]    Script Date: 25-07-2023 14:04:28 ******/
DROP PROCEDURE [dbo].[SP_WHStockManagement]
GO
/****** Object:  StoredProcedure [dbo].[SP_WarehouseManagement]    Script Date: 25-07-2023 14:04:28 ******/
DROP PROCEDURE [dbo].[SP_WarehouseManagement]
GO
/****** Object:  StoredProcedure [dbo].[SP_UserManagement]    Script Date: 25-07-2023 14:04:28 ******/
DROP PROCEDURE [dbo].[SP_UserManagement]
GO
/****** Object:  StoredProcedure [dbo].[SP_TaskManagement]    Script Date: 25-07-2023 14:04:28 ******/
DROP PROCEDURE [dbo].[SP_TaskManagement]
GO
/****** Object:  StoredProcedure [dbo].[SP_ReimbursementManagement]    Script Date: 25-07-2023 14:04:28 ******/
DROP PROCEDURE [dbo].[SP_ReimbursementManagement]
GO
/****** Object:  StoredProcedure [dbo].[SP_MasterManagement]    Script Date: 25-07-2023 14:04:28 ******/
DROP PROCEDURE [dbo].[SP_MasterManagement]
GO
/****** Object:  StoredProcedure [dbo].[SP_LeaveManagement]    Script Date: 25-07-2023 14:04:28 ******/
DROP PROCEDURE [dbo].[SP_LeaveManagement]
GO
/****** Object:  StoredProcedure [dbo].[SP_AutoScheduleAttendance]    Script Date: 25-07-2023 14:04:28 ******/
DROP PROCEDURE [dbo].[SP_AutoScheduleAttendance]
GO
/****** Object:  StoredProcedure [dbo].[SP_AttendanceManagement]    Script Date: 25-07-2023 14:04:28 ******/
DROP PROCEDURE [dbo].[SP_AttendanceManagement]
GO
/****** Object:  UserDefinedFunction [dbo].[Fun_GetTotalProductCount]    Script Date: 25-07-2023 14:04:28 ******/
DROP FUNCTION [dbo].[Fun_GetTotalProductCount]
GO
/****** Object:  UserDefinedFunction [dbo].[fun_GetProductCountDetails]    Script Date: 25-07-2023 14:04:28 ******/
DROP FUNCTION [dbo].[fun_GetProductCountDetails]
GO
/****** Object:  UserDefinedFunction [dbo].[fun_GetProductCountDetails]    Script Date: 25-07-2023 14:04:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION [dbo].[fun_GetProductCountDetails] 
(
	-- Add the parameters for the function here
	@WareHouseId int,
	@ProductId int,
	@SubProductId int,
	@ProductQty decimal(12,2)
)
RETURNS 
    @WHStock table(TransferType nvarchar(50),
	ProductAddQty decimal(12,2),
	StockIn decimal(12,2),
	StockOut decimal(12,2),
	TotalQty decimal(12,2)
	)
AS
BEGIN
	-- Fill the table variable with the rows for your result set

	DECLARE @ResultVar decimal(12,2),
	        @InStock decimal(12,2),
			@OutStock decimal(12,2),
			@TransferType nvarchar(50)
	-- Add the T-SQL statements to compute the return value here
	if not exists(select isnull(Sum(TransferQuantity),0) from WHStockTransferManage where  DestinationId=@WareHouseId and ProductId=@ProductId and SubProductId=@SubProductId and TransferType in(1,3))
	begin
	set @InStock=0.0
	set @TransferType='NA'
	end
	else
	begin
	set @InStock=(select isnull(Sum(TransferQuantity),0) from WHStockTransferManage where  DestinationId=@WareHouseId and ProductId=@ProductId and SubProductId=@SubProductId and TransferType in(1,3))
	--set @TransferType=()
	end

	if not exists(select isnull(Sum(TransferQuantity),0) from WHStockTransferManage where  SourceId=@WareHouseId and ProductId=@ProductId and SubProductId=@SubProductId and TransferType in(1,2))
	begin
	set @OutStock=0.0
	end
	else
	begin
	set @OutStock=(select isnull(Sum(TransferQuantity),0) from WHStockTransferManage where  SourceId=@WareHouseId and ProductId=@ProductId and SubProductId=@SubProductId and TransferType in(1,2))
	end
	
	--set @TransferType=(select isnull(TransferType,0) from WHStockTransferManage where  SourceId=@WareHouseId and ProductId=@ProductId and SubProductId=@SubProductId)
	--select isnull(Sum(TransferQuantity),0) from WHStockTransferManage where  SourceId=1 and ProductId=1 and SubProductId=2
	
	set @TransferType='Na'
	set @ResultVar=(@ProductQty+ @InStock-@OutStock)

	insert into @WHStock(TransferType,ProductAddQty,StockIn,StockOut,TotalQty) values(@TransferType,@ProductQty,@InStock,@OutStock,@ResultVar)
	
	RETURN 
END
GO
/****** Object:  UserDefinedFunction [dbo].[Fun_GetTotalProductCount]    Script Date: 25-07-2023 14:04:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Chandan>
-- Create date: <21/6/2023>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[Fun_GetTotalProductCount] 
(
	-- Add the parameters for the function here
	@WareHouseId int,
	@ProductId int,
	@SubProductId int,
	@ProductQty decimal(12,2)
	
)
RETURNS  decimal(12,2)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @ResultVar decimal(12,2),
	        @InStock decimal(12,2),
			@OutStock decimal(12,2)
	-- Add the T-SQL statements to compute the return value here
	if not exists(select isnull(Sum(TransferQuantity),0) from WHStockTransferManage where  DestinationId=@WareHouseId and ProductId=@ProductId and SubProductId=@SubProductId and TransferType in(1,3))
	begin
	set @InStock=0.0
	end
	else
	begin
	set @InStock=(select isnull(Sum(TransferQuantity),0) from WHStockTransferManage where  DestinationId=@WareHouseId and ProductId=@ProductId and SubProductId=@SubProductId and TransferType in(1,3))
	end

	if not exists(select isnull(Sum(TransferQuantity),0) from WHStockTransferManage where  SourceId=@WareHouseId and ProductId=@ProductId and SubProductId=@SubProductId and TransferType in(1,2))
	begin
	set @OutStock=0.0
	end
	else
	begin
	set @OutStock=(select isnull(Sum(TransferQuantity),0) from WHStockTransferManage where  SourceId=@WareHouseId and ProductId=@ProductId and SubProductId=@SubProductId and TransferType in(1,2))
	end
	
	--select isnull(Sum(TransferQuantity),0) from WHStockTransferManage where  SourceId=1 and ProductId=1 and SubProductId=2
	set @ResultVar=(@ProductQty+ @InStock-@OutStock)
	-- Return the result of the function
	RETURN @ResultVar

END
GO
/****** Object:  StoredProcedure [dbo].[SP_AttendanceManagement]    Script Date: 25-07-2023 14:04:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--exec SP_AttendanceManagement @ActionId=4,@AttendanceId=0,@UserId=0 @DeviationsLoginTime='07:08',@DeviationsLogoutTime='09:09'
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SP_AttendanceManagement] 
	-- Add the parameters for the stored procedure here
	@ActionId int=0
   ,@AttendanceId int=0
   ,@UserId int=null
   ,@Date date=null
   ,@LoginTime time(0)=null
   ,@LogoutTime time(0)=null
   ,@DurationTime time(0)=null
   ,@Reason nvarchar(max)=null
   ,@AttendanceStatusId int=0
   ,@CreatedBy int=0
   ,@DeviationsLoginTime time(0)=null
   ,@DeviationsLogoutTime time(0)=null
   ,@DeviationsDurationTime time(0)=null
   ,@DeviationsApprovalStatusId int=0
   ,@AttendanceApproveRejectBy int=0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	
	IF(@ActionId=1)
	BEGIN
	INSERT INTO [dbo].[Attendance]
           ([UserId]
           ,[Date]
           ,[LoginTime]
           ,[LogoutTime]
           ,[DurationTime]
           ,[Reason]
           ,[AttendanceStatusId]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[UpdatedBy]
           ,[UpdateDate]
           ,[IsActive])
     VALUES
           (@UserId
           ,@Date
           ,@LoginTime 
           ,@LogoutTime 
           ,@DurationTime 
           ,@Reason 
           ,@AttendanceStatusId 
           ,@CreatedBy 
           ,GETDATE()
           ,@CreatedBy
           ,GETDATE()
           ,1)

	END

    if(@ActionId=2)
	begin
	  UPDATE [dbo].[Attendance]
   SET [UserId] = @UserId
      ,[Date] = @Date
      ,[LoginTime] = @LoginTime
      ,[LogoutTime] = @LogoutTime
      ,[DurationTime] = @DurationTime
      ,[Reason] = @Reason
      ,[AttendanceStatusId] = @AttendanceStatusId      
      ,[UpdatedBy] = @CreatedBy
      ,[UpdateDate] =GETDATE()
      
  WHERE [AttendanceId] =@AttendanceId

	end

	if(@ActionId=3)
	begin
	 UPDATE [dbo].[Attendance] set [IsActive]=0 WHERE [AttendanceId] =@AttendanceId
	end

	if(@ActionId=4)
	begin
	if(@UserId=0)
	begin
	 set @UserId=null
	end

	SELECT a.[AttendanceId]
      ,a.[UserId]
	  ,b.Name as 'UserName'
      ,CONVERT(varchar, A.[Date], 34) as [Date]
      ,a.[LoginTime]
      ,a.[LogoutTime]
      ,a.[DurationTime]
      ,a.[Reason]
      ,a.[AttendanceStatusId]
	  ,d.Value as 'AttendanceStatus'
	  ,a.[DeviationsLoginTime]
      ,a.[DeviationsLogoutTime]
      ,a.[DeviationsDurationTime]
      ,isnull(a.[DeviationsApprovalStatusId],0) as [DeviationsApprovalStatusId]
	  ,e.Value as DeviationsApprovalStatus
      ,a.[CreatedBy]
      ,a.[CreatedDate]  as 'CreatedDateTime'    
  FROM [dbo].[Attendance] as a 
  left join [dbo].[User] as b on b.UserId=a.[UserId]
  left join [dbo].[User] as c on c.UserId=a.[CreatedBy]
  left join [dbo].[SysVal] as d on d.Id=a.[AttendanceStatusId]
  left join [dbo].[SysVal] as e on e.Id=a.[DeviationsApprovalStatusId]
  where a.IsActive=1 and a.[UserId]=ISNULL(@UserId,a.[UserId])
	end

	if(@ActionId=5)
	begin
	UPDATE [dbo].[Attendance]
   SET 
       [DeviationsLoginTime] = convert(varchar, @DeviationsLoginTime, 8)
      ,[DeviationsLogoutTime] =convert(varchar, @DeviationsLogoutTime, 8) 
      ,[DeviationsDurationTime] =@DeviationsDurationTime   
	  ,[Reason] =isnull([Reason],@Reason)
      ,[UpdatedBy] = @CreatedBy
      ,[UpdateDate] = GETDATE()      
 WHERE [AttendanceId]=@AttendanceId

	end

	if(@ActionId=6)
	begin
	UPDATE [dbo].[Attendance]
   SET [DeviationsApprovalStatusId]=@DeviationsApprovalStatusId
      ,[AttendanceApproveRejectBy]=@AttendanceApproveRejectBy        
   WHERE [AttendanceId]=@AttendanceId

	end

END
GO
/****** Object:  StoredProcedure [dbo].[SP_AutoScheduleAttendance]    Script Date: 25-07-2023 14:04:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--exec [dbo].[SP_AutoScheduleAttendance] @ActionId=1
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SP_AutoScheduleAttendance]
    -- Add the parameters for the stored procedure here
    @ActionId int = 0
AS
BEGIN

    INSERT INTO [dbo].[Attendance]
    (
        [UserId],
        [Date],
        [AttendanceStatusId],
        [CreatedBy],
        [CreatedDate],
        [IsActive]
    )
    select UserId,
           convert(varchar, getdate(), 23),
           5,
           1,
           GETDATE(),
           1
    from [dbo].[User] 
    where RoleId = 4
          and IsActive = 1  
		  and UserId not in(select UserId from Attendance where [Date]=convert(varchar, getdate(), 23))

		  select @@ROWCOUNT
END


--select * from [Attendance]
GO
/****** Object:  StoredProcedure [dbo].[SP_LeaveManagement]    Script Date: 25-07-2023 14:04:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SP_LeaveManagement]
	-- Add the parameters for the stored procedure here
	@ActionId int=0
	,@UserId int=0
	,@LeaveId int=0
	,@StartDate date=null
	,@EndDate date=null
	,@LeaveReason nvarchar(max)=null
	,@LeaveType int=0
	,@Document nvarchar(100)=null
	,@LeaveStatus int=0
	,@LeaveApproveRejectBy int=0
	,@LeaveRejectedReason nvarchar(max)=null
	,@CreatedBy int=0
	,@NumberOfleaveCount nvarchar(100)=null
AS
BEGIN
	
	if(@ActionId=1)
	begin

	INSERT INTO [dbo].[LeaveApply]
           ([UserId]
           ,[StartDate]
           ,[EndDate]
           ,[LeaveReason]
           ,[LeaveType]
           ,[Document]
           ,[LeaveStatus]
           ,[LeaveApproveRejectBy]
           ,[LeaveRejectedReason]
		   ,NumberOfleaveCount
           ,[IsActive])
     VALUES
           (@UserId
           ,@StartDate
           ,@EndDate
           ,@LeaveReason
           ,@LeaveType
           ,@Document
           ,@LeaveStatus
           ,@LeaveApproveRejectBy
           ,@LeaveRejectedReason
		   ,@NumberOfleaveCount
           ,1)

	end

	if(@ActionId=2)
	begin

	if(@UserId=0)
	begin
	set @UserId=null
	end

	 SELECT [LeaveId]
      ,a.[UserId]
	  ,b.Name as UserName
      ,CONVERT(varchar, A.[StartDate], 34) as [StartDate]
      ,CONVERT(varchar, A.[EndDate], 34) as [EndDate]
	  --,DATEDIFF(day, A.[StartDate], A.[EndDate]) +1 as NumberOfleaveCount
	  ,NumberOfleaveCount
      ,[LeaveReason]
      ,isnull([LeaveType],0) as [LeaveType]
	  ,c.Value as LeaveTypeValue
      ,[Document]
      ,isnull([LeaveStatus],0) as [LeaveStatus]
	  ,d.Value as LeaveStatusValue
      ,[LeaveApproveRejectBy]
	  ,e.Name as LeaveApproveRejectByName
      ,[LeaveRejectedReason]
      ,a.[IsActive]
     FROM [dbo].[LeaveApply] as a 
	 left join [dbo].[User] as b on b.UserId=a.[UserId]
	 left join [dbo].[SysVal] as c on c.Id=a.[LeaveType]
	 left join [dbo].[SysVal] as d on d.Id=a.[LeaveStatus]
	 left join [dbo].[User] as e on e.UserId=a.[LeaveApproveRejectBy]
	 where a.IsActive=1 and a.UserId=isnull(@UserId,a.UserId)
	end

	if(@ActionId=3)
	begin 

	UPDATE [dbo].[LeaveApply]
   SET [LeaveStatus]=@LeaveStatus
      ,[LeaveApproveRejectBy]=@LeaveApproveRejectBy
	  ,[LeaveRejectedReason]=@LeaveRejectedReason
   where [LeaveId]=@LeaveId
	end
	
END
GO
/****** Object:  StoredProcedure [dbo].[SP_MasterManagement]    Script Date: 25-07-2023 14:04:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Chandan Mandal>
-- Create date: <23-06-2023>
-- Description:	<All Master Management Action>
-- =============================================
CREATE PROCEDURE [dbo].[SP_MasterManagement]
	-- Add the parameters for the stored procedure here
	@ActionId bigint=0
   ,@ProductId int=0
   ,@ProductName nvarchar(100)=null
   ,@Specifications nvarchar(max)=null
   ,@UnitId int=0
   ,@SubProductId int=0
   ,@SubProductName nvarchar(100)=null
   ,@DepartmentId int=0
   ,@DepartmentName nvarchar(50)=null
   ,@DepartmentHead int=0

AS
BEGIN
	
	if(@ActionId=1)
	BEGIN
	   SELECT [ID]
      ,[Name]
      ,[CountryCode]
       FROM [dbo].[CountryMaster]
	END

	--********** // Insert Product Master //************
	if(@ActionId=2)
	begin
	   INSERT INTO [dbo].[ProductMaster]
           ([ProductName]
           ,[Specifications]
           ,[UnitId]
           ,[IsActive])
     VALUES
           (@ProductName
           ,@Specifications
           ,@UnitId
           ,1)

	end

	--***********// Update Product Master //**************
	if(@ActionId=3)
	begin
	  UPDATE [dbo].[ProductMaster]
      SET [ProductName] = @ProductName
         ,[Specifications] = @Specifications
         ,[UnitId] = @UnitId      
     WHERE [ProductId]=@ProductId
	end

	--**********// Delete Product Master //**************
	IF(@ActionId=4)
	BEGIN
	 UPDATE [dbo].[ProductMaster] SET [IsActive]=0 WHERE [ProductId]=@ProductId
	END

	--*********// Insert Sub Product //****************
	IF(@ActionId=5)
	BEGIN
	INSERT INTO [dbo].[SubProductMaster]
           ([ProductId]
           ,[SubProductName]
           ,[UnitId]
           ,[IsActive])
     VALUES
           (@ProductId
           ,@SubProductName
           ,@UnitId
           ,1)
    END

	--************// Update Sub Product //***************
	IF(@ActionId=6)
	BEGIN
	UPDATE [dbo].[SubProductMaster]
   SET [ProductId] = @ProductId
      ,[SubProductName] = @SubProductName
      ,[UnitId] = @UnitId      
   WHERE [SubProductId]=@SubProductId
   END

   --***********// Delete Sub Product //***************
   IF(@ActionId=7)
   BEGIN
     UPDATE [dbo].[SubProductMaster] SET [IsActive]=0 WHERE [SubProductId]=@SubProductId
   END

   --************// Insert Department //*****************
   IF(@ActionId=8)
   BEGIN
   INSERT INTO [dbo].[Department]
           ([DepartmentName]
           ,[DepartmentHead]
           ,[IsActive])
     VALUES
           (@DepartmentName
           ,@DepartmentHead
           ,1)
   END

   --***********// Update Department //*********************
   IF(@ActionId=9)
   BEGIN
   UPDATE [dbo].[Department]
   SET [DepartmentName] = @DepartmentName
      ,[DepartmentHead] = @DepartmentHead      
   WHERE [DepartmentId]=@DepartmentId

   END

   --***********// Delete Department //*********************
   IF(@ActionId=10)
   BEGIN
   UPDATE [dbo].[Department]
   SET [IsActive] = 0
   WHERE [DepartmentId]=@DepartmentId
   END

END
GO
/****** Object:  StoredProcedure [dbo].[SP_ReimbursementManagement]    Script Date: 25-07-2023 14:04:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SP_ReimbursementManagement]
	-- Add the parameters for the stored procedure here
	 @ActionId int=0
	,@Id int=0
	,@UserId int=0
    ,@ExpenseFor nvarchar(max)=null
    ,@TotalExpenseAmount nvarchar(50)=null
    ,@ClaimAmount nvarchar(50)=null
    ,@ApprovedAmount nvarchar(50)=null
    ,@ExpenseDate datetime=null
    ,@ExpenseDocument nvarchar(50)=null
	,@Signature nvarchar(50)=null
    ,@ApprovedBy int=null
    ,@ApprovedDate datetime=null
    ,@StatusId int=null
    ,@ClaimBy int=null
    ,@Comments nvarchar(max)=null
	,@CreatedBy int=null
	,@ExpenseToDate datetime=null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	if(@ActionId=1)
	begin
	  INSERT INTO [dbo].[Reimbursement]
           ([ExpenseFor]
           ,[TotalExpenseAmount]
           ,[ClaimAmount]
           ,[ApprovedAmount]
           ,[ExpenseDate]
		   ,ExpenseToDate
           ,[ExpenseDocument]
		   ,[Signature]
           ,[ApprovedBy]
           ,[ApprovedDate]
           ,[StatusId]
           ,[ClaimBy]
           ,[Comments]
           ,[CreatedDate]
           ,[CreatedBy]
           ,[UpdateDate]
           ,[UpdatedBy]
           ,[IsActive])
     VALUES
           (@ExpenseFor 
           ,@TotalExpenseAmount 
           ,@ClaimAmount 
           ,@ApprovedAmount 
           ,@ExpenseDate 
		   ,@ExpenseToDate
           ,@ExpenseDocument 
		   ,@Signature
           ,@ApprovedBy 
           ,@ApprovedDate 
           ,@StatusId 
           ,@ClaimBy 
           ,@Comments 
           ,getdate()
           ,@CreatedBy 
           ,getdate()
           ,@CreatedBy
           ,1)

		   --select @@IDENTITY as Id
		    select SCOPE_IDENTITY() as Id

	end
	if(@ActionId=2)
	begin

	 UPDATE [dbo].[Reimbursement]
   SET [ExpenseFor] = @ExpenseFor
      ,[TotalExpenseAmount] = @TotalExpenseAmount
      ,[ClaimAmount] = @ClaimAmount
      ,[ApprovedAmount] = @ApprovedAmount
      ,[ExpenseDate] = @ExpenseDate
	  ,[ExpenseToDate]=@ExpenseToDate
      ,[ExpenseDocument] = @ExpenseDocument
	  ,[Signature]=@Signature
      ,[ApprovedBy] = @ApprovedBy
      ,[ApprovedDate] = @ApprovedDate
      ,[StatusId] = @StatusId
      ,[ClaimBy] = @ClaimBy
      ,[Comments] = @Comments      
      ,[UpdateDate] = GETDATE()
      ,[UpdatedBy] = @CreatedBy
     
 WHERE [Id]=@Id
	end

	if(@ActionId=3)
	begin

	if(@UserId=0)
	begin
	set @UserId=null
	end

	  SELECT a.[Id]
      ,a.[ExpenseFor]
      ,a.[TotalExpenseAmount]
      ,a.[ClaimAmount]
      ,a.[ApprovedAmount]
      ,CONVERT(varchar, A.[ExpenseDate], 34) as [ExpenseDate]
	  ,CONVERT(varchar, A.[ExpenseToDate], 34) as ExpenseToDate
      ,a.[ExpenseDocument]
	  ,a.[Signature]
      ,a.[ApprovedBy]
	  ,b.[Name] as ApprovedByName
      ,CONVERT(varchar, A.[ApprovedDate], 34) as [ApprovedDate]
      ,a.[StatusId]
	  ,c.[Value] as [Status]
      ,a.[ClaimBy]
	  ,d.[Name] as ClaimByName
      ,a.[Comments]
      ,a.[CreatedDate]
      ,a.[CreatedBy]      
    FROM [dbo].[Reimbursement] as a
	left join [dbo].[User] as b on b.UserId=a.[ApprovedBy]
	left join [dbo].[SysVal] as c on c.Id=a.[StatusId]
	left join [dbo].[User] as d on d.UserId=a.[ClaimBy]
	where a.[IsActive]=1 and a.[ClaimBy]=isnull(@UserId,a.[ClaimBy])

	end

	if(@ActionId=4)
	begin

	 UPDATE [dbo].[Reimbursement]
    SET 
       [ApprovedBy] = @ApprovedBy
      ,[ApprovedDate] = GETDATE()
      ,[StatusId] = @StatusId      
      ,[Comments] = @Comments      
    WHERE [Id]=@Id

	select @Id as Id
	end

	if(@ActionId=5)
	begin
	INSERT INTO [dbo].[ReimbursementDocument]
           ([ReimbursementId]
           ,[DocumentName]
           ,[UploadedBy]
           ,[UploadedDate]
           ,[IsActive])
     VALUES
           (@Id
           ,@ExpenseDocument
           ,@CreatedBy
           ,getdate()
           ,1)

		   select @Id as Id

	end

	if(@ActionId=6)
	begin
	SELECT [DocumentId]
      ,[ReimbursementId]
      ,[DocumentName]
	  ,'Upload' as DocumentUrl
      ,[UploadedBy]
      ,CONVERT(varchar, [UploadedDate], 34) as [UploadedDate]
      
   FROM [dbo].[ReimbursementDocument] where [IsActive]=1 and [ReimbursementId]=@Id

	end

	if(@ActionId=7)
	begin

	delete from [dbo].[ReimbursementDocument] where [DocumentId]=@Id
	select @Id as Id
	end

	if(@ActionId=8)
	begin

	UPDATE [dbo].[Reimbursement]
    SET 
       [IsActive]=0
      ,[UpdateDate] = GETDATE()
      ,[UpdatedBy] = @CreatedBy     
    WHERE [Id]=@Id

	select @Id as Id
	end

END
GO
/****** Object:  StoredProcedure [dbo].[SP_TaskManagement]    Script Date: 25-07-2023 14:04:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--exec [dbo].[SP_TaskManagement] 4,0
-- =============================================
-- Author:  	Chandan
-- Create date: 10/07/2023
-- Description:	Task Management
-- =============================================
CREATE PROCEDURE [dbo].[SP_TaskManagement]
    -- Add the parameters for the stored procedure here
    @ActionId        int           = 0,
    @TaskId          int           = NULL,
    @AssignTo        int           = NULL,
    @TaskName        nvarchar(50)  = NULL,
    @TaskDueDateTime nvarchar(50)  = NULL,
    @ProjectId       int           = 0,
    @ProjectHeadId   int           = 0,
    @SiteId          int           = 0,
    @TaskPriority    nvarchar(20)  = NULL,
    @TaskDescription nvarchar(max) = NULL,
    @TaskDocument    nvarchar(50)  = NULL,
    @TaskStatus      int           = 0,
    @CreatedBy       int           = 0,
    @Comment         nvarchar(max) = NULL,
    @SpentTime       nvarchar(50)  = NULL
AS
    BEGIN
        -- SET NOCOUNT ON added to prevent extra result sets from
        -- interfering with SELECT statements.
        IF (@ActionId = 1)
            BEGIN

                INSERT INTO [dbo].[Task]
                    (
                        [AssignTo],
                        [TaskName],
                        [TaskDueDateTime],
                        [ProjectId],
                        [ProjectHeadId],
                        [SiteId],
                        [TaskPriority],
                        [TaskDescription],
                        [TaskDocument],
                        [TaskStatus],
                        [CreatedBy],
                        [CreatedDate],
                        [UpdatedBy],
                        [UpdateDate],
                        [IsActive]
                    )
                VALUES
                    (
                        @AssignTo,
                        @TaskName,
                        @TaskDueDateTime,
                        @ProjectId,
                        @ProjectHeadId,
                        @SiteId,
                        @TaskPriority,
                        @TaskDescription,
                        @TaskDocument,
                        @TaskStatus,
                        @CreatedBy,
                        GETDATE(),
                        @CreatedBy,
                        GETDATE(),
                        1
                    )
            END

        IF (@ActionId = 2)
            BEGIN

                UPDATE
                    [dbo].[Task]
                SET
                    [AssignTo] = @AssignTo,
                    [TaskName] = @TaskName,
                    [TaskDueDateTime] = @TaskDueDateTime,
                    [ProjectId] = @ProjectId,
                    [ProjectHeadId] = @ProjectHeadId,
                    [SiteId] = @SiteId,
                    [TaskPriority] = @TaskPriority,
                    [TaskDescription] = @TaskDescription,
                    [TaskDocument] = @TaskDocument,
                    [TaskStatus] = @TaskStatus,
                    [UpdatedBy] = @CreatedBy,
                    [UpdateDate] = GETDATE()
                WHERE
                    [TaskId] = @TaskId
            END

        IF (@ActionId = 3)
            BEGIN
                UPDATE
                    [dbo].[Task]
                SET
                    [IsActive] = 0,
                    [UpdatedBy] = @CreatedBy,
                    [UpdateDate] = GETDATE()
                WHERE
                    [TaskId] = @TaskId
            END

        IF (@ActionId = 4)
            BEGIN

                IF (@TASKID = 0)
                    BEGIN
                        SET @TASKID = NULL
                    END

                IF (@ASSIGNTO = 0)
                    BEGIN
                        SET @ASSIGNTO = NULL
                    END


                SELECT
                    a.[TaskId],
                    a.[AssignTo],
                    b.[Name]      AS [AssignToName],
                    a.[TaskName],
                    a.[TaskDueDateTime],
                    a.[ProjectId],
                    'ProjectName' AS ProjectName,
                    a.[ProjectHeadId],
                    c.[Name]      AS [ProjectHeadName],
                    a.[SiteId],
                    d.SiteName,
                    a.[TaskPriority],
                    a.[TaskDescription],
                    a.[TaskDocument],
                    a.[TaskStatus],
                    e.[Value]     AS [TaskStatusValue],
                    a.[CreatedBy],
                    a.[CreatedDate],
                    a.[UpdatedBy],
                    a.[UpdateDate],
                    a.[IsActive]
                FROM
                    [dbo].[Task]       AS a
                    LEFT JOIN
                        [dbo].[User]   AS b
                            ON b.UserId = a.[AssignTo]
                    LEFT JOIN
                        [dbo].[User]   AS c
                            ON c.UserId = a.[ProjectHeadId]
                    LEFT JOIN
                        [dbo].[Site]   AS d
                            ON d.SiteId = a.[SiteId]
                    LEFT JOIN
                        [dbo].[SysVal] AS e
                            ON e.Id = a.[TaskStatus]
                WHERE
                    a.[IsActive] = 1
                    AND a.[TaskId] = ISNULL(@TaskId, a.[TaskId])
                    AND a.[AssignTo] = ISNULL(@AssignTo, a.[AssignTo])
            END


        IF (@ActionId = 5)
            BEGIN


                SELECT
                    [TaskUpdateId],
                    [TaskId],
                    [Comment],
                    [SpentTime],
                    [TaskDocument],
                    [TaskStatus],
                    a.[CreatedBy],
                    b.[Name]                               AS CreatedByName,
                    CONVERT(varchar, A.[CreatedDate], 101) AS CreatedDateTime,
                    A.[UpdatedBy],
                    A.[UpdateDate],
                    e.[Value]                              AS [TaskStatusValue]
                FROM
                    [dbo].[TaskUpdate] AS A
                    LEFT JOIN
                        [dbo].[SysVal] AS e
                            ON e.Id = a.[TaskStatus]
                    LEFT JOIN
                        [dbo].[User]   AS b
                            ON b.UserId = a.[CreatedBy]
                WHERE
                    a.[IsActive] = 1
                    AND a.[TaskId] = @TaskId


            END

        IF (@ActionId = 6)
            BEGIN



                INSERT INTO [dbo].[TaskUpdate]
                    (
                        [TaskId],
                        [Comment],
                        [SpentTime],
                        [TaskDocument],
                        [TaskStatus],
                        [CreatedBy],
                        [CreatedDate],
                        [UpdatedBy],
                        [UpdateDate],
                        [IsActive]
                    )
                VALUES
                    (
                        @TaskId,
                        @Comment,
                        @SpentTime,
                        @TaskDocument,
                        @TaskStatus,
                        @CreatedBy,
                        GETDATE(),
                        @CreatedBy,
                        GETDATE(),
                        1
                    )

            END



    END
GO
/****** Object:  StoredProcedure [dbo].[SP_UserManagement]    Script Date: 25-07-2023 14:04:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--exec [dbo].[SP_UserManagement] @ActionId=4,@UserId=16,@RoleId=0
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SP_UserManagement]
	-- Add the parameters for the stored procedure here
	 @ActionId int=0
    ,@UserId int=0
    ,@BusinessName nvarchar(50)=null
    ,@Name nvarchar(50)=null
    ,@Email nvarchar(50)=null
    ,@Password nvarchar(50)=null
    ,@ContactNo nvarchar(20)=null
    ,@RoleId int=0
    ,@CreatedBy int=0
	,@JobTitle nvarchar(50)=null
	,@UserCode nvarchar(50)=null
	,@DepartmentId int=0
	,@ReportingTo int=0
	,@LoginTime nvarchar(10)=null
	,@LogoutTime nvarchar(10)=null
	,@IsFlexibleTime bit=0
	,@MobileNo nvarchar(20)=null
	,@LandlineNo nvarchar(20)=null
	,@AlternativeNo nvarchar(20)=null
	,@PermanentAddress nvarchar(max)=null
	,@PresentAddress nvarchar(max)=null
	,@JoiningDate nvarchar(20)=null

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	
	IF(@ActionId=1)
	BEGIN

	if(@RoleId in (1,4))
	begin
	 set @BusinessName='SIFSL'
	end
	

	   INSERT INTO [dbo].[User]
           (
           [BusinessName]
           ,[Name]
           ,[Email]
           ,[Password]
           ,[ContactNo]
		   ,[MobileNo]
		   ,[LandlineNo]
		   ,[AlternativeNo]
           ,[RoleId]
		   ,[JobTitle]
           ,[UserCode]
           ,[DepartmentId]
           ,[ReportingTo]
           ,[LoginTime]
           ,[LogoutTime]
		   ,[IsFlexibleTime]
		   ,[PermanentAddress]
		   ,[PresentAddress]
		   ,[JoiningDate]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[UpdatedBy]
           ,[UpdateDate]
           ,[IsActive])
     VALUES
           (
            @BusinessName
           ,@Name 
           ,@Email 
           ,@Password 
           ,@ContactNo 
		   ,@MobileNo
		   ,@LandlineNo
		   ,@AlternativeNo
           ,@RoleId 
		   ,@JobTitle
		   ,@UserCode
		   ,@DepartmentId
		   ,@ReportingTo
		   ,@LoginTime
		   ,@LogoutTime
		   ,@IsFlexibleTime
		   ,@PermanentAddress
		   ,@PresentAddress
		   ,@JoiningDate
           ,@CreatedBy 
           ,GETDATE()
           ,@CreatedBy
           ,GETDATE()
           ,1)
	END

	IF(@ActionId=2)
	BEGIN

	   UPDATE [dbo].[User]
       SET 
       [BusinessName] = @BusinessName
      ,[Name] = @Name
      ,[Email] = @Email
      ,[Password] = @Password
      ,[ContactNo] = @ContactNo
	  ,[MobileNo]=@MobileNo
      ,[LandlineNo]=@LandlineNo
	  ,[AlternativeNo]=@AlternativeNo
      ,[RoleId] = @RoleId   
	  ,[JobTitle]=@JobTitle
      ,[UserCode]=@UserCode
      ,[DepartmentId]=@DepartmentId
      ,[ReportingTo]=@ReportingTo
      ,[LoginTime]=@LoginTime
      ,[LogoutTime]=@LogoutTime
	  ,[IsFlexibleTime]=@IsFlexibleTime
	  ,[PermanentAddress]=@PermanentAddress
	  ,[PresentAddress]=@PresentAddress
	  ,[JoiningDate]=@JoiningDate
      ,[UpdatedBy] = @CreatedBy
      ,[UpdateDate] = GETDATE()      
       WHERE [UserId] = @UserId
	END

	IF(@ActionId=3)
	BEGIN
	   UPDATE [dbo].[User] SET IsActive=0,[UpdatedBy] = @CreatedBy,[UpdateDate] = GETDATE()  WHERE  [UserId] = @UserId
	END

	IF(@ActionId=4)
	BEGIN
	
	if(@UserId=0)
	begin
	set @UserId=null
	end
	if(@RoleId=0)
	begin
	set @RoleId=null
	end

	SELECT [UserId]
      ,[BusinessName]
      ,[Name]
      ,[Email]
      ,[Password]
      ,[ContactNo]
	  ,[MobileNo]
	  ,[LandlineNo]
	  ,[AlternativeNo]
      ,isnull(a.[RoleId],cast(0 as int)) as RoleId
	  ,b.RoleName
      ,[JobTitle]
      ,[UserCode]
      ,isnull(a.[DepartmentId],cast(0 as int)) as DepartmentId
	  ,c.DepartmentName
      ,isnull([ReportingTo],cast(0 as int)) as ReportingTo
      ,[LoginTime]
      ,[LogoutTime]
	  ,[IsFlexibleTime]
	  ,[PermanentAddress]
	  ,[PresentAddress]
	  ,[JoiningDate]
      ,[CreatedBy]
      ,[CreatedDate]
      ,[UpdatedBy]
      ,[UpdateDate]
      ,a.[IsActive]
    FROM [dbo].[User] as a
	left join [dbo].[UserRole] as b on b.UserRoleId=a.RoleId
	left join [dbo].[Department] as c on c.DepartmentId=a.DepartmentId
	where a.UserId=ISNULL(@UserId, a.UserId) and a.RoleId=ISNULL(@RoleId,a.RoleId) 
	and a.IsActive=1
	END


END
GO
/****** Object:  StoredProcedure [dbo].[SP_WarehouseManagement]    Script Date: 25-07-2023 14:04:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 --exec [dbo].[SP_WarehouseManagement] @ActionId=4,@TransferType=1,@SourceId=3,@DestinationId=2,@ProductId=1,@SubProductId=1,@Quantity=1000
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SP_WarehouseManagement] 
	-- Add the parameters for the stored procedure here
	@ActionId bigint=0,
	@Id int=0,
	@WarehouseId bigint=0,
	@WarehouseName nvarchar(50)=null,
	@Address nvarchar(50)=null,
	@CityId int=0,
	@StateId int=0,
	@CountryId int=0,
	@ZipCode nvarchar(20)=null,
	@Block nvarchar(50)=null,
	@AdminName nvarchar(50)=null,
	@AdminEmailId  nvarchar(50)=null,
	@AdminContactNo nvarchar(50)=null,	
	@Status int=0,
	@StartDate datetime=null,
	@EndDate datetime=null,
	@AdminId int=0,
    @ProductId int=0,
    @SubProductId int=0,
    @UnitId int=0,
    @Quantity decimal(12,2)=0.0,
    @SupplierId int=0,
	@UnitPrice decimal(12,2)=0.0,
    @Amount decimal(12,2)=0.0,
	@CreatedBy int=0,

	@TransferType int=0,
	@SourceId int=0,
    @DestinationId int=0,
	@TransferDate datetime=null,
	@Comment nvarchar(max)=null,
	@CreatedDate datetime=null



AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	
	if(@ActionId=1)
	begin

	declare @WareHouseAdminId int=0
	
	INSERT INTO [dbo].[WareHouse]
           ([WareHouseName]
           ,[Address]
           ,[StateId]
           ,[CityId]
           ,[Block]
           ,[ZipCode]
           ,[CountryId]
           ,[WareHouseAdminId]
           ,[IsActive])
     VALUES
           (@WarehouseName
           ,@Address         
           ,@StateId
		   ,@CityId
           ,@Block
           ,@ZipCode
           ,@CountryId
           ,@AdminId
           ,1)

		select @WareHouseId = Scope_Identity() 


		INSERT INTO [dbo].[WareHouseAdminDetails]
           ([WareHouseId]
           ,[UserId]
           ,[UserName]
           ,[Email]
           ,[ContactNumber]
           ,[Status]
           ,[StartDate]
           ,[EndDate]
           ,[IsActive])
     VALUES
           (@WareHouseId
           ,@AdminId
           ,@AdminName
           ,@AdminEmailId
           ,@AdminContactNo
           ,@Status
           ,GETDATE()
           ,GETDATE()
           ,1)

		   select @WareHouseAdminId = Scope_Identity() 

		   --update  [dbo].[WareHouse] set [WareHouseAdminId]=@WareHouseAdminId where WareHouseId=@WareHouseId

		   end

		   if(@ActionId=2)
		   begin

		   INSERT INTO [dbo].[WHStockProduct]
           ([WareHouseId]
           ,[ProductId]
           ,[SubProductId]
           ,[UnitId]
           ,[Quantity]
           ,[SupplierId]
		   ,[UnitPrice]
           ,[Amount]
		   ,[Comment]
           ,[CreatedDate]
           ,[CreatedBy]
           ,[UpdateDate]
           ,[UpdatedBy]
           ,[IsActive])
     VALUES
           (@WareHouseId
           ,@ProductId
           ,@SubProductId
           ,@UnitId
           ,@Quantity
           ,@SupplierId
		   ,@UnitPrice
           ,@Amount
		   ,@Comment
           ,@CreatedDate
           ,@CreatedBy
           ,GETDATE()
           ,@CreatedBy
           ,1)

		   end

		   if(@ActionId=3)
		   begin


		   INSERT INTO [dbo].[WHStockTransferManage]
           ([TransferType]
           ,[SourceId]
           ,[DestinationId]
           ,[ProductId]
           ,[SubProductId]
           ,[TransferQuantity]
		   ,[Comment]
           ,[TransferDate]
           ,[TransferBy]
           ,[UpdateDate]
           ,[UpdatedBy]
           ,[IsActive])
     VALUES
           (@TransferType
           ,@SourceId
           ,@DestinationId
           ,@ProductId
           ,@SubProductId
           ,@Quantity
		   ,@Comment
           ,@TransferDate
           ,@CreatedBy
           ,GETDATE()
           ,@CreatedBy
           ,1)

		   end

		   if(@ActionId=4)
		   begin
		   declare  @LiveProductQuantity decimal(12,2)
		   Declare @WHStock table(WareHouseId int,ProductId int,SubProductId int,Quantity decimal(12,2))

		   insert into @WHStock 
		   select WareHouseId,ProductId,SubProductId,SUM(Quantity) 
		   from WHStockProduct 
		   where WareHouseId=@SourceId 
		   and ProductId=@ProductId 
		   and SubProductId=@SubProductId		   
		   group by WareHouseId,ProductId,SubProductId

		   select @LiveProductQuantity=Quantity from @WHStock

		   if(@TransferType=3)
		   begin
		     set @LiveProductQuantity=5000
		   end

		   if(@Quantity<=@LiveProductQuantity)
		   begin
		     select @LiveProductQuantity as Quantity
		   end
		   else
		   begin 
		      select cast(-1 as decimal(12,2)) as  Quantity
		   end

		   end


		   if(@ActionId=5)
		   begin
		   UPDATE [dbo].[WHStockProduct]
   SET [WareHouseId] = @WareHouseId
      ,[ProductId] =@ProductId
      ,[SubProductId] = @SubProductId
      ,[UnitId] = @UnitId
      ,[Quantity] = @Quantity
      ,[SupplierId] = @SupplierId
      ,[UnitPrice] = @UnitPrice
      ,[Amount] = @Amount     
      ,[UpdateDate] = getdate()
      ,[UpdatedBy] = @CreatedBy
      
 WHERE [Id]=@Id

		   end

		   if(@ActionId=6)
		   begin
		   update [dbo].[WHStockProduct] set IsActive=0 where [Id]=@Id
		   end

		   if(@ActionId=7)
		   begin
		   declare @OldAdminId int=0,@OldWareHouseAdminId int=0
		   if not exists(select top 1 isnull([UserId],0) from [dbo].[WareHouseAdminDetails] where [WareHouseId]=@WarehouseId and IsActive=1 order by [WareHouseAdminId] desc)
		   begin
		      set @OldAdminId=0
		   end
		   else
		   begin
		     set @OldAdminId=(select top 1 isnull([UserId],0) from [dbo].[WareHouseAdminDetails] where [WareHouseId]=@WarehouseId and IsActive=1 order by [WareHouseAdminId] desc)
		   end
		   
		   if(@OldAdminId!=@AdminId)
			begin
			set @OldWareHouseAdminId=(select top 1 [WareHouseAdminId] from [dbo].[WareHouseAdminDetails] where [WareHouseId]=@WarehouseId and IsActive=1 order by [WareHouseAdminId] desc)
			update [dbo].[WareHouseAdminDetails]  set IsActive=0,[EndDate]=GETDATE() WHERE [WareHouseAdminId]=@OldWareHouseAdminId
			INSERT INTO [dbo].[WareHouseAdminDetails]
           ([WareHouseId]
           ,[UserId]
           ,[UserName]
           ,[Email]
           ,[ContactNumber]
           ,[Status]
           ,[StartDate]
           ,[EndDate]
           ,[IsActive])
     VALUES
           (@WareHouseId
           ,@AdminId
           ,@AdminName
           ,@AdminEmailId
           ,@AdminContactNo
           ,@Status
           ,GETDATE()
           ,GETDATE()
           ,1)

			end

			UPDATE [dbo].[WareHouse]
   SET [WareHouseName] = @WareHouseName
      ,[Address] = @Address
      ,[StateId] = @StateId
      ,[CityId] = @CityId
      ,[Block] = @Block
      ,[ZipCode] = @ZipCode
      ,[CountryId] = @CountryId
      ,[WareHouseAdminId] = @AdminId      
   WHERE [WareHouseId]=@WareHouseId

   

 --  update [dbo].[WareHouseAdminDetails]  set IsActive=0 WHERE [WareHouseId]=@WareHouseId

 --  if not exists(select * from [dbo].[WareHouseAdminDetails] WHERE [WareHouseId]=@WareHouseId and [UserId]=@AdminId)
 --  begin    
 --      INSERT INTO [dbo].[WareHouseAdminDetails]
 --          ([WareHouseId]
 --          ,[UserId]
 --          ,[UserName]
 --          ,[Email]
 --          ,[ContactNumber]
 --          ,[Status]
 --          ,[StartDate]
 --          ,[EndDate]
 --          ,[IsActive])
 --    VALUES
 --          (@WareHouseId
 --          ,@AdminId
 --          ,@AdminName
 --          ,@AdminEmailId
 --          ,@AdminContactNo
 --          ,@Status
 --          ,GETDATE()
 --          ,GETDATE()
 --          ,1)   
		    
 --  end
 --  else
 --  begin
 --    UPDATE [dbo].[WareHouseAdminDetails]
 --  SET [UserName] = @AdminName
 --     ,[Email] = @AdminEmailId
 --     ,[ContactNumber] = @AdminContactNo
 --     ,[Status] = @Status
 --     ,[StartDate] = GETDATE()
 --     ,[EndDate] = GETDATE()      
 --WHERE [WareHouseId] = @WareHouseId and [UserId] = @AdminId
 --  end


	   end


END
GO
/****** Object:  StoredProcedure [dbo].[SP_WHStockManagement]    Script Date: 25-07-2023 14:04:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
---exec dbo.SP_WHStockManagement @ActionId=1,@Id=0
-- =============================================
-- Author:		<Chandan>
-- Create date: <21-06-2023>
-- Description:	<for fetch stock management details>
-- =============================================
CREATE PROCEDURE [dbo].[SP_WHStockManagement]
	-- Add the parameters for the stored procedure here
	@ActionId int=0,
	@Id int=0,
	@WareHouseId int=0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	if(@ActionId=1)
	begin

	  --select * from [dbo].[WHStockProduct]


	       Declare @WHStock table(WareHouseId int,ProductId int,SubProductId int,Quantity decimal(12,2),
		                          WareHouseName nvarchar(50),ProductName nvarchar(50),SubProductName nvarchar(50))

		   insert into @WHStock 
		   select  a.WareHouseId,a.ProductId,a.SubProductId,SUM(a.Quantity),max(d.WareHouseName),Max(b.ProductName),max(c.SubProductName) 
		   from WHStockProduct  as a
		   join [dbo].[ProductMaster] b on b.[ProductId]=a.ProductId
		   join [dbo].[SubProductMaster] c on c.SubProductId=a.SubProductId
		   join [dbo].[WareHouse] d on d.WareHouseId=a.WareHouseId
		   where a.IsActive=1		   
		   group by a.WareHouseId,a.ProductId,a.SubProductId

		   select WareHouseId,WareHouseName,ProductId,ProductName,SubProductId,SubProductName,--Quantity,
		  (select dbo.Fun_GetTotalProductCount(WareHouseId,ProductId,SubProductId,Quantity) ) as Quantity
		  ,(select TransferType from dbo.fun_GetProductCountDetails(WareHouseId,ProductId,SubProductId,Quantity)) as TransferType
		  ,(select ProductAddQty from dbo.fun_GetProductCountDetails(WareHouseId,ProductId,SubProductId,Quantity)) as ProductAddQty
		  ,(select StockIn from dbo.fun_GetProductCountDetails(WareHouseId,ProductId,SubProductId,Quantity)) as StockIn
		  ,(select StockOut from dbo.fun_GetProductCountDetails(WareHouseId,ProductId,SubProductId,Quantity)) as StockOut
		  ,(select TotalQty from dbo.fun_GetProductCountDetails(WareHouseId,ProductId,SubProductId,Quantity)) as TotalQty
		   from @WHStock		                          
		  
	end


	if(@ActionId=2)
	begin

	  
	       Declare @WHStock1 table(WareHouseId int,ProductId int,SubProductId int,Quantity decimal(12,2),
		                          WareHouseName nvarchar(50),ProductName nvarchar(50),SubProductName nvarchar(50))

		   insert into @WHStock1 
		   select  a.WareHouseId,a.ProductId,a.SubProductId,SUM(a.Quantity),max(d.WareHouseName),Max(b.ProductName),max(c.SubProductName) 
		   from WHStockProduct  as a
		   join [dbo].[ProductMaster] b on b.[ProductId]=a.ProductId
		   join [dbo].[SubProductMaster] c on c.SubProductId=a.SubProductId
		   join [dbo].[WareHouse] d on d.WareHouseId=a.WareHouseId
		   where a.IsActive=1	and a.WareHouseId=@WareHouseId   
		   group by a.WareHouseId,a.ProductId,a.SubProductId

		   select WareHouseId,WareHouseName,ProductId,ProductName,SubProductId,SubProductName,--Quantity,
		  (select dbo.Fun_GetTotalProductCount(WareHouseId,ProductId,SubProductId,Quantity) ) as Quantity
		  ,(select TransferType from dbo.fun_GetProductCountDetails(WareHouseId,ProductId,SubProductId,Quantity)) as TransferType
		  ,(select ProductAddQty from dbo.fun_GetProductCountDetails(WareHouseId,ProductId,SubProductId,Quantity)) as ProductAddQty
		  ,(select StockIn from dbo.fun_GetProductCountDetails(WareHouseId,ProductId,SubProductId,Quantity)) as StockIn
		  ,(select StockOut from dbo.fun_GetProductCountDetails(WareHouseId,ProductId,SubProductId,Quantity)) as StockOut
		  ,(select TotalQty from dbo.fun_GetProductCountDetails(WareHouseId,ProductId,SubProductId,Quantity)) as TotalQty
		   from @WHStock1	 	   

		  
	end
END

GO
/****** Object:  StoredProcedure [dbo].[uspUserDetails]    Script Date: 25-07-2023 14:04:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[uspUserDetails] 
	-- Add the parameters for the stored procedure here
	@UserId int =0
	
AS    
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select * from [User] 
END
GO
