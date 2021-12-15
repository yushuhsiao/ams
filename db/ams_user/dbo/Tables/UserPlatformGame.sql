CREATE TABLE [dbo].[UserPlatformGame] (
    [CorpId]     INT      NOT NULL,
    [PlatformId] INT      NOT NULL,
    [GameId]     INT      NOT NULL,
    [UserId]     BIGINT   NOT NULL,
    [Flags]      SMALLINT NOT NULL,
    CONSTRAINT [PK_UserPlatformGame] PRIMARY KEY CLUSTERED ([GameId] ASC, [UserId] ASC)
);

