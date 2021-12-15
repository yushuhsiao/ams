CREATE TABLE [dbo].[Password] (
    [UserId]     BIGINT       NOT NULL,
    [ver]        ROWVERSION   NOT NULL,
    [Encrypt]    INT          NOT NULL,
    [a]          VARCHAR (50) NULL,
    [b]          VARCHAR (50) NULL,
    [c]          VARCHAR (50) NULL,
    [Expiry]     INT          NULL,
    [ExpireTime] AS           (dateadd(month,[Expiry],[CreateTime])),
    [CreateTime] DATETIME     CONSTRAINT [DF_Password_CreateTime] DEFAULT (getutcdate()) NOT NULL,
    [CreateUser] BIGINT       NOT NULL,
    CONSTRAINT [PK_Password] PRIMARY KEY CLUSTERED ([UserId] ASC)
);



