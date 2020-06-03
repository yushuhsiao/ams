CREATE TABLE [dbo].[Users_Admin] (
    [Id]        INT NOT NULL,
    [ManagerId] INT NULL,
    CONSTRAINT [PK_Users_Admin] PRIMARY KEY CLUSTERED ([Id] ASC)
);

