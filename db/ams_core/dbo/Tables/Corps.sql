CREATE TABLE [dbo].[Corps] (
    [Id]          INT           NOT NULL,
    [Name]        VARCHAR (20)  NOT NULL,
    [Active]      TINYINT       CONSTRAINT [DF_Corps_Active] DEFAULT ((1)) NOT NULL,
    [Currency]    SMALLINT      NOT NULL,
    [DisplayName] NVARCHAR (20) NOT NULL,
    [CreateTime]  DATETIME      CONSTRAINT [DF_Corp_CreateTime] DEFAULT (getutcdate()) NULL,
    [CreateUser]  BIGINT        CONSTRAINT [DF_Corp_CreateUser] DEFAULT ((0)) NULL,
    [ModifyTime]  DATETIME      CONSTRAINT [DF_Corp_ModifyTime] DEFAULT (getutcdate()) NULL,
    [ModifyUser]  BIGINT        CONSTRAINT [DF_Corp_ModifyUser] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_Corps] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [IX_Corps] UNIQUE NONCLUSTERED ([Name] ASC)
);



