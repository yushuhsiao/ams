CREATE TABLE [dbo].[CurrencyHist] (
    [A]          SMALLINT        NOT NULL,
    [B]          SMALLINT        NOT NULL,
    [ver]        BIGINT          NOT NULL,
    [X]          DECIMAL (19, 6) NOT NULL,
    [ModifyTime] DATETIME        CONSTRAINT [DF_CurrencyHist_ModifyTime] DEFAULT (getdate()) NOT NULL,
    [ModifyUser] INT             CONSTRAINT [DF_CurrencyHist_ModifyUser] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_CurrencyHist] PRIMARY KEY CLUSTERED ([A] ASC, [B] ASC, [ver] ASC)
);

