CREATE TABLE [dbo].[Game] (
    [Id]             INT           NOT NULL,
    [GamePlatformId] INT           NOT NULL,
    [GameTypeId]     INT           NULL,
    [Name]           VARCHAR (50)  NOT NULL,
    [OriginalName]   NVARCHAR (50) NULL,
    [CreateTime]     DATETIME      CONSTRAINT [DF_Game_CreateTime] DEFAULT (getutcdate()) NOT NULL,
    [CreateUser]     INT           CONSTRAINT [DF_Game_CreateUser] DEFAULT ((0)) NOT NULL,
    [ModifyTime]     DATETIME      CONSTRAINT [DF_Game_ModifyTime] DEFAULT (getutcdate()) NOT NULL,
    [ModifyUser]     INT           CONSTRAINT [DF_Game_ModifyUser] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Game] PRIMARY KEY CLUSTERED ([Id] ASC)
);



