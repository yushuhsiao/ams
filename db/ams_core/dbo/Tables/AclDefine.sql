CREATE TABLE [dbo].[AclDefine] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [Path]         VARCHAR (200) NOT NULL,
    [Flag]         INT           NOT NULL,
    [DefaultFlags] INT           NOT NULL,
    CONSTRAINT [PK_AclDefine] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [IX_AclDefine] UNIQUE NONCLUSTERED ([Path] ASC)
);

