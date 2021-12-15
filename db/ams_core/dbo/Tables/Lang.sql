CREATE TABLE [dbo].[Lang] (
    [PlatformId] INT           CONSTRAINT [DF_Lang_PlatfrmId] DEFAULT ((0)) NOT NULL,
    [Path]       VARCHAR (200) NOT NULL,
    [Type]       VARCHAR (50)  NOT NULL,
    [Key]        VARCHAR (50)  NOT NULL,
    [LCID]       INT           NOT NULL,
    [Text]       NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Lang] PRIMARY KEY CLUSTERED ([PlatformId] ASC, [LCID] ASC, [Path] ASC, [Type] ASC, [Key] ASC)
);

