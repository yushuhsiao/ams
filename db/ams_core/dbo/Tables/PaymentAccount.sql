CREATE TABLE [dbo].[PaymentAccount] (
    [Id]          INT            NOT NULL,
    [Name]        VARCHAR (20)   NOT NULL,
    [PaymentId]   INT            NOT NULL,
    [Active]      TINYINT        NOT NULL,
    [MerhantId]   VARCHAR (50)   NOT NULL,
    [extdata]     VARCHAR (MAX)  NULL,
    [Description] NVARCHAR (MAX) NULL,
    [CreateTime]  DATETIME       CONSTRAINT [DF_PaymentAccount_CreateTime] DEFAULT (getutcdate()) NOT NULL,
    [CreateUser]  BIGINT         CONSTRAINT [DF_PaymentAccount_CreateUser] DEFAULT ((0)) NOT NULL,
    [ModifyTime]  DATETIME       CONSTRAINT [DF_PaymentAccount_ModifyTime] DEFAULT (getutcdate()) NOT NULL,
    [ModifyUser]  BIGINT         CONSTRAINT [DF_PaymentAccount_ModifyUser] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_PaymentAccount] PRIMARY KEY CLUSTERED ([Id] ASC)
);



