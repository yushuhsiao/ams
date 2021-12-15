CREATE PROCEDURE UpdateBalance
	@UserId bigint,
	@Amount1 decimal(19, 6),
	@Amount2 decimal(19, 6),
	@Amount3 decimal(19, 6)
AS
BEGIN
	declare @p1 decimal(19, 6), @p2 decimal(19, 6), @p3 decimal(19, 6), @ver timestamp

	select @ver = ver, @p1 = Balance1, @p2 = Balance2, @p3 = Balance3
	from UserBalance
	where Id = @UserId

	if @ver is null
	begin
		insert into UserBalance (Id, Balance1, Balance2, Balance3) values (@UserId, 0, 0, 0)
		select @p1 = 0, @p2 = 0, @p3 = 0, @ver = 0
	end

	update UserBalance
	set Balance1 = Balance1 + @Amount1,
		Balance2 = Balance2 + @Amount2,
		Balance3 = Balance3 + @Amount3
	where Id = @UserId

	select
		Id				as UserId,
		Balance,
		@p1				as PrevBalance1,
		Balance1 - @p1	as Amount1,
		Balance1,
		@p2				as PrevBalance2,
		Balance2 - @p2	as Amount2,
		Balance2,
		@p3				as PrevBalance3,
		Balance3 - @p3	as Amount3,
		Balance3,
		@ver		as ver
	from UserBalance
	where Id = @UserId
END