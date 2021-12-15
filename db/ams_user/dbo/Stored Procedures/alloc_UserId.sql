CREATE procedure [dbo].[alloc_UserId]
	@CorpId int,
	@UserType tinyint,
	@UserName varchar(20)
as
begin
	declare @uid uniqueidentifier
	set @uid = newid()
	insert into UserId ([uid], CorpId, UserType, UserName)
	values             (@uid ,@CorpId,@UserType,@UserName)
	select Id from UserId nolock
	where [uid] = @uid
end