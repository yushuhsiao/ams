CREATE TABLE [dbo].[Platforms] (
    [Id]           INT          NOT NULL,
    [Name]         VARCHAR (20) NOT NULL,
    [PlatformType] INT          NOT NULL,
    [Currency]     SMALLINT     NOT NULL,
    [Active]       TINYINT      NOT NULL,
    [CreateTime]   DATETIME     CONSTRAINT [DF_Platforms_CreateTime] DEFAULT (getutcdate()) NOT NULL,
    [CreateUser]   BIGINT       CONSTRAINT [DF_Platforms_CreateUser] DEFAULT ((0)) NOT NULL,
    [ModifyTime]   DATETIME     CONSTRAINT [DF_Platforms_ModifyTime] DEFAULT (getutcdate()) NOT NULL,
    [ModifyUser]   BIGINT       CONSTRAINT [DF_Platforms_ModifyUser] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Platforms] PRIMARY KEY CLUSTERED ([Id] ASC)
);



