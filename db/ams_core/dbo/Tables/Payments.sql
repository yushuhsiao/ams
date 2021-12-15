CREATE TABLE [dbo].[Payments] (
    [Id]          INT           NOT NULL,
    [PaymentType] INT           NOT NULL,
    [Name]        NVARCHAR (50) NOT NULL,
    [Active]      TINYINT       NOT NULL,
    [CreateTime]  DATETIME      CONSTRAINT [DF_Payments_CreateTime] DEFAULT (getutcdate()) NOT NULL,
    [CreateUser]  BIGINT        CONSTRAINT [DF_Payments_CreateUser] DEFAULT ((0)) NOT NULL,
    [ModifyTime]  DATETIME      CONSTRAINT [DF_Payments_ModifyTime] DEFAULT (getutcdate()) NOT NULL,
    [ModifyUser]  BIGINT        CONSTRAINT [DF_Payments_ModifyUser] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Payments] PRIMARY KEY CLUSTERED ([Id] ASC)
);



