CREATE TABLE [dbo].[Users_General] (
    [Id]        INT NOT NULL,
    [MaxDepth]  INT CONSTRAINT [DF_Users_General_MaxDepth] DEFAULT ((0)) NULL,
    [MaxUsers]  INT NULL,
    [MaxAdmins] INT NULL,
    CONSTRAINT [PK_Users_General] PRIMARY KEY CLUSTERED ([Id] ASC)
);

