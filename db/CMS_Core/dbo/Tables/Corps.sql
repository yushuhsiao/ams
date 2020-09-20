CREATE TABLE [dbo].[Corps] (
    [Id]         INT          NOT NULL,
    [ver]        ROWVERSION   NOT NULL,
    [Name]       VARCHAR (20) NOT NULL,
    [Mode]       INT          NOT NULL,
    [Active]     TINYINT      DEFAULT ((255)) NOT NULL,
    [Currency]   SMALLINT     NOT NULL,
    [Prefix]     VARCHAR (10) DEFAULT ('') NOT NULL,
    [CreateTime] DATETIME     DEFAULT (getdate()) NOT NULL,
    [CreateUser] INT          NOT NULL,
    [ModifyTime] DATETIME     DEFAULT (getdate()) NOT NULL,
    [ModifyUser] INT          NOT NULL,
    CONSTRAINT [PK_Corps] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [IX_Corps] UNIQUE NONCLUSTERED ([Id] ASC)
);

