CREATE PROCEDURE UpdatePassword
	@UserId		bigint,
	@Encrypt	int,
	@a			varchar(50),
	@b			varchar(50),
	@c			varchar(50),
	@x			varchar(100) = null,
	@Expiry		int = null,
	@CreateUser int
AS
BEGIN
	insert into [PasswordHist] ([UserId], [ver], [Encrypt], [a], [b], [c], [Expiry], [CreateTime], [CreateUser], [x])
	select      [UserId], convert(bigint, [ver]),[Encrypt], [a], [b], [c], [Expiry], [CreateTime], [CreateUser], @x
	from [Password]
	where UserId = @UserId

	delete from [Password]
	where UserId = @UserId

	insert into [Password] ([UserId], [Encrypt], [a], [b], [c], [Expiry], [CreateTime], [CreateUser])
	values                 (@UserId , @Encrypt , @a , @b , @c , @Expiry , getdate()   , @CreateUser)
END