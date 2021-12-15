CREATE TABLE [dbo].[Agents] (
    [Id]          BIGINT        NOT NULL,
    [CorpId]      INT           NOT NULL,
    [Name]        VARCHAR (20)  NOT NULL,
    [Active]      TINYINT       CONSTRAINT [DF_Agents_Active] DEFAULT ((1)) NOT NULL,
    [ParentId]    BIGINT        NOT NULL,
    [DisplayName] NVARCHAR (20) NULL,
    [Depth]       INT           NOT NULL,
    [MaxDepth]    INT           CONSTRAINT [DF_Agents_MaxDepth] DEFAULT ((0)) NOT NULL,
    [MaxAgents]   INT           NULL,
    [MaxAdmins]   INT           NULL,
    [MaxMembers]  INT           NULL,
    [CreateTime]  DATETIME      CONSTRAINT [DF_Agents_CreateTime] DEFAULT (getutcdate()) NULL,
    [CreateUser]  BIGINT        CONSTRAINT [DF_Agents_CreateUser] DEFAULT ((0)) NULL,
    [ModifyTime]  DATETIME      CONSTRAINT [DF_Agents_ModifyTime] DEFAULT (getutcdate()) NULL,
    [ModifyUser]  BIGINT        CONSTRAINT [DF_Agents_ModifyUser] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_Agents] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [IX_Agents] UNIQUE NONCLUSTERED ([CorpId] ASC, [Name] ASC)
);



