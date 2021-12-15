create procedure [dbo].[TableVer_get] @name varchar(30), @index int as
begin
	select _ver from TableVer nolock where _name=@name and _index=@index
end