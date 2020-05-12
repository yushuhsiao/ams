-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [TableVer_get] @db varchar(20), @table varchar(30), @index int as
begin
	select _ver from TableVer nolock where _db=@db and _table=@table and _index=@index
end