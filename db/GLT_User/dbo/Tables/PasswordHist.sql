CREATE TABLE [dbo].[PasswordHist] (
    [UserId]     INT           NOT NULL,
    [ver]        BIGINT        NOT NULL,
    [Encrypt]    INT           NOT NULL,
    [a]          VARCHAR (50)  NULL,
    [b]          VARCHAR (50)  NULL,
    [c]          VARCHAR (50)  NULL,
    [Expiry]     INT           NULL,
    [ExpireTime] AS            (dateadd(month,[Expiry],[CreateTime])),
    [CreateTime] DATETIME      NOT NULL,
    [CreateUser] BIGINT        NOT NULL,
    [x]          VARCHAR (100) NULL,
    CONSTRAINT [PK_PasswordHist] PRIMARY KEY CLUSTERED ([UserId] ASC, [ver] ASC)
);

