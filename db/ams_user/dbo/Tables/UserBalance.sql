CREATE TABLE [dbo].[UserBalance] (
    [Id]       BIGINT          NOT NULL,
    [ver]      ROWVERSION      NOT NULL,
    [Balance1] DECIMAL (19, 6) NOT NULL,
    [Balance2] DECIMAL (19, 6) NOT NULL,
    [Balance3] DECIMAL (19, 6) NOT NULL,
    [Balance]  AS              (([Balance1]+[Balance2])+[Balance3]),
    CONSTRAINT [PK_UserBalance] PRIMARY KEY CLUSTERED ([Id] ASC)
);

