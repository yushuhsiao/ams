-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [TableVer_set] @db varchar(20), @table varchar(30), @index int as
begin
	if exists (select _ver from TableVer nolock where _db=@db and _table=@table and _index=@index)
	update TableVer set _time=getdate() where _db=@db and _table=@table and _index=@index
	else insert into TableVer (_db, _table, _index) values (@db, @table, @index)
	select _ver from TableVer nolock where _db=@db and _table=@table and _index=@index
end