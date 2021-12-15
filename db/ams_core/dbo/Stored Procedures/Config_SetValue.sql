CREATE procedure [dbo].[Config_SetValue]
	@CorpId int,
	@PlatformId int,
	@Key1 varchar(20),
	@Key2 varchar(20),
	@Value varchar(1000)
as
begin
	if exists (select Id from Config nolock where CorpId=@CorpId and PlatformId=@PlatformId and Key1=@Key1 and Key2=@Key2)
		update Config set Value=@Value
		where CorpId=@CorpId and PlatformId=@PlatformId and Key1=@Key1 and Key2=@Key2
	else
		insert into Config (CorpId, PlatformId, Key1, Key2, Value)
		values (@CorpId, @PlatformId, @Key1, @Key2, @Value)
	select * from Config nolock
	where CorpId=@CorpId and PlatformId=@PlatformId and Key1=@Key1 and Key2=@Key2
end