CREATE TABLE [dbo].[AgentPlatform] (
    [AgentId]    BIGINT NOT NULL,
    [PlatformId] INT    NOT NULL,
    CONSTRAINT [PK_AgentPlatform] PRIMARY KEY CLUSTERED ([AgentId] ASC, [PlatformId] ASC)
);

