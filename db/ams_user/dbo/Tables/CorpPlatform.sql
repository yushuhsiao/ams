CREATE TABLE [dbo].[CorpPlatform] (
    [CorpId]     INT NOT NULL,
    [PlatformId] INT NOT NULL,
    CONSTRAINT [PK_CorpPlatform] PRIMARY KEY CLUSTERED ([CorpId] ASC, [PlatformId] ASC)
);

