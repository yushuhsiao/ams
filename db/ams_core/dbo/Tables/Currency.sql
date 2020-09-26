CREATE TABLE [dbo].[Currency] (
    [A]          SMALLINT        NOT NULL,
    [B]          SMALLINT        NOT NULL,
    [ver]        ROWVERSION      NOT NULL,
    [X]          DECIMAL (19, 6) NOT NULL,
    [ModifyTime] DATETIME        CONSTRAINT [DF_Currency_ModifyTime] DEFAULT (getdate()) NOT NULL,
    [ModifyUser] INT             CONSTRAINT [DF_Currency_ModifyUser] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Currency] PRIMARY KEY CLUSTERED ([A] ASC, [B] ASC)
);

