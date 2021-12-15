CREATE TABLE [dbo].[Admins] (
    [Id]          BIGINT        NOT NULL,
    [CorpId]      INT           NOT NULL,
    [Name]        VARCHAR (20)  NOT NULL,
    [Active]      TINYINT       CONSTRAINT [DF_Admins_Active] DEFAULT ((1)) NOT NULL,
    [ParentId]    BIGINT        NOT NULL,
    [DisplayName] NVARCHAR (20) NULL,
    [Depth]       INT           NOT NULL,
    [CreateTime]  DATETIME      CONSTRAINT [DF_Admins_CreateTime] DEFAULT (getutcdate()) NULL,
    [CreateUser]  BIGINT        CONSTRAINT [DF_Admins_CreateUser] DEFAULT ((0)) NULL,
    [ModifyTime]  DATETIME      CONSTRAINT [DF_Admins_ModifyTime] DEFAULT (getutcdate()) NULL,
    [ModifyUser]  BIGINT        CONSTRAINT [DF_Admins_ModifyUser] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_Admins] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [IX_Admins] UNIQUE NONCLUSTERED ([CorpId] ASC, [Name] ASC)
);



