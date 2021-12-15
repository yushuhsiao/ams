CREATE TABLE [dbo].[MemberGame] (
    [MemberId] BIGINT NOT NULL,
    [GameId]   INT    NOT NULL,
    CONSTRAINT [PK_MemberGame] PRIMARY KEY CLUSTERED ([MemberId] ASC, [GameId] ASC)
);

