create procedure [dbo].[TableVer_set] @name varchar(30), @index int as
begin
	if exists (select _ver from TableVer nolock where _name=@name and _index=@index)
	update TableVer set _time=getdate() where _name=@name and _index=@index
	else insert into TableVer (_name, _index) values (@name, @index)
	select _ver from TableVer nolock where _name=@name and _index=@index
end