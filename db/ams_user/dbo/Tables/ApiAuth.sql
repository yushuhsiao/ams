CREATE TABLE [dbo].[ApiAuth] (
    [UserId]   BIGINT        NOT NULL,
    [Active]   TINYINT       NOT NULL,
    [AuthType] INT           NOT NULL,
    [Arg1]     VARCHAR (MAX) NOT NULL,
    [Arg2]     VARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_ApiAuth] PRIMARY KEY CLUSTERED ([UserId] ASC)
);

