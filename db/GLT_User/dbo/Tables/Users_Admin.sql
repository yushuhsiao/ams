CREATE TABLE [dbo].[Users_Admin] (
    [Id]        INT NOT NULL,
    [ManagerId] INT NOT NULL,
    CONSTRAINT [PK_Users_Admin] PRIMARY KEY CLUSTERED ([Id] ASC)
);



