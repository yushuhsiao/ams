CREATE PROCEDURE [dbo].[Lang_Set]
	@PlatformId	int,
	@Path		varchar(200),
	@Type		varchar(50),
	@Key		varchar(50),
	@LCID		int,
	@Text		nvarchar(50)
AS
BEGIN

if exists
(
	select [Text] from [Lang] nolock
	where [PlatformId] = @PlatformId and [Path] = @Path and [Type] = @Type and [Key] = @Key and [LCID] = @LCID
)
	update [Lang]
	set [Text] = @Text
	where [PlatformId] = @PlatformId and [Path] = @Path and [Type] = @Type and [Key] = @Key and [LCID] = @LCID
else
	insert into [Lang] ([PlatformId], [Path], [Type], [Key], [LCID], [Text])
	values (@PlatformId, @Path, @Type, @Key, @LCID, @Text)
END