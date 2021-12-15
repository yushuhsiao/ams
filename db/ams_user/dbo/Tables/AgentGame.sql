CREATE TABLE [dbo].[AgentGame] (
    [AgentId] BIGINT NOT NULL,
    [GameId]  INT    NOT NULL,
    CONSTRAINT [PK_AgentGame] PRIMARY KEY CLUSTERED ([AgentId] ASC, [GameId] ASC)
);

