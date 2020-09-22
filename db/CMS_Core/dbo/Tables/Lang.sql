CREATE TABLE [dbo].[Lang] (
    [Category] VARCHAR (200)  NOT NULL,
    [Name]     VARCHAR (50)   NOT NULL,
    [LCID]     INT            NOT NULL,
    [Text]     NVARCHAR (255) NULL,
    PRIMARY KEY CLUSTERED ([Category] ASC, [Name] ASC, [LCID] ASC)
);

