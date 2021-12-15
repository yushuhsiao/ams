CREATE TABLE [dbo].[UserId] (
    [Id]       BIGINT           IDENTITY (1, 1) NOT NULL,
    [uid]      UNIQUEIDENTIFIER NOT NULL,
    [CorpId]   INT              NOT NULL,
    [UserType] TINYINT          NOT NULL,
    [UserName] VARCHAR (20)     NOT NULL,
    CONSTRAINT [PK_UserId] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [IX_UserId] UNIQUE NONCLUSTERED ([uid] ASC),
    CONSTRAINT [IX_UserId_1] UNIQUE NONCLUSTERED ([CorpId] ASC, [UserType] ASC, [UserName] ASC)
);

