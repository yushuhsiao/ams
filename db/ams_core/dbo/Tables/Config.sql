CREATE TABLE [dbo].[Config] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [CorpId]      INT            NOT NULL,
    [Key1]        VARCHAR (20)   NOT NULL,
    [Key2]        VARCHAR (20)   NOT NULL,
    [Value]       NVARCHAR (200) NULL,
    [Description] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Config] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [IX_Config] UNIQUE NONCLUSTERED ([CorpId] ASC, [Key1] ASC, [Key2] ASC)
);

