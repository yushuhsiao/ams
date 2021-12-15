CREATE TABLE [dbo].[CorpGame] (
    [CorpId] INT NOT NULL,
    [GameId] INT NOT NULL,
    CONSTRAINT [PK_CorpGame] PRIMARY KEY CLUSTERED ([CorpId] ASC, [GameId] ASC)
);

