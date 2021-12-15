CREATE TABLE [dbo].[Config] (
    [Id]          BIGINT         IDENTITY (1, 1) NOT NULL,
    [CorpId]      INT            NOT NULL,
    [PlatformId]  INT            CONSTRAINT [DF_Table_1_PlatformID] DEFAULT ((0)) NOT NULL,
    [Key1]        VARCHAR (20)   NOT NULL,
    [Key2]        VARCHAR (20)   NOT NULL,
    [Value]       VARCHAR (1000) NOT NULL,
    [Description] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Config] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [IX_Config] UNIQUE NONCLUSTERED ([CorpId] ASC, [PlatformId] ASC, [Key1] ASC, [Key2] ASC)
);

