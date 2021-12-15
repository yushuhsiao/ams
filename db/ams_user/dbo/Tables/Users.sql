CREATE TABLE [dbo].[Users] (
    [Id]          BIGINT        NOT NULL,
    [UserType]    TINYINT       NOT NULL,
    [CorpId]      INT           NOT NULL,
    [Name]        VARCHAR (20)  NOT NULL,
    [Active]      TINYINT       CONSTRAINT [DF_Users_Active] DEFAULT ((1)) NOT NULL,
    [ParentId]    BIGINT        NOT NULL,
    [DisplayName] NVARCHAR (20) NULL,
    [Depth]       INT           NOT NULL,
    [CreateTime]  DATETIME      CONSTRAINT [DF_Users_CreateTime] DEFAULT (getutcdate()) NULL,
    [CreateUser]  BIGINT        CONSTRAINT [DF_Users_CreateUser] DEFAULT ((0)) NULL,
    [ModifyTime]  DATETIME      CONSTRAINT [DF_Users_ModifyTime] DEFAULT (getutcdate()) NULL,
    [ModifyUser]  BIGINT        CONSTRAINT [DF_Users_ModifyUser] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [IX_Users] UNIQUE NONCLUSTERED ([CorpId] ASC, [Name] ASC)
);



