CREATE TABLE [dbo].[AclDefine] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [ParentId] INT           NULL,
    [Name]     VARCHAR (20)  NOT NULL,
    [FullName] VARCHAR (100) NULL,
    CONSTRAINT [PK_AclDefine] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [IX_AclDefine] UNIQUE NONCLUSTERED ([ParentId] ASC, [Name] ASC)
);

