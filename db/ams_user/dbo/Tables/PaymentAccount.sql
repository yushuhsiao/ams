CREATE TABLE [dbo].[PaymentAccount] (
    [Id]               INT          NOT NULL,
    [PaymentAccountId] INT          NOT NULL,
    [CorpId]           INT          NOT NULL,
    [AgentId]          INT          NOT NULL,
    [Name]             VARCHAR (20) NOT NULL,
    [Active]           TINYINT      NOT NULL,
    [CreateTime]       DATETIME     CONSTRAINT [DF_PaymentAccount_CreateTime] DEFAULT (getutcdate()) NOT NULL,
    [CreateUser]       BIGINT       NOT NULL,
    [ModifyTime]       DATETIME     CONSTRAINT [DF_PaymentAccount_ModifyTime] DEFAULT (getutcdate()) NOT NULL,
    [ModifyUser]       BIGINT       NOT NULL,
    CONSTRAINT [PK_PaymentAccount] PRIMARY KEY CLUSTERED ([Id] ASC)
);



