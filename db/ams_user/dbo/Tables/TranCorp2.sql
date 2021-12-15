CREATE TABLE [dbo].[TranCorp2] (
    [TranId]       UNIQUEIDENTIFIER NOT NULL,
    [ver]          BIGINT           NOT NULL,
    [LogType]      INT              NOT NULL,
    [SerialNumber] VARCHAR (16)     NOT NULL,
    [CorpId]       INT              NOT NULL,
    [CorpName]     VARCHAR (20)     NOT NULL,
    [Amount1]      DECIMAL (19, 6)  NOT NULL,
    [Amount2]      DECIMAL (19, 6)  NOT NULL,
    [Amount3]      DECIMAL (19, 6)  NOT NULL,
    [CurrencyA]    SMALLINT         NOT NULL,
    [CurrencyB]    SMALLINT         NOT NULL,
    [CurrencyX]    DECIMAL (19, 6)  NOT NULL,
    [RequestIP]    VARCHAR (20)     NOT NULL,
    [RequestTime]  DATETIME         CONSTRAINT [DF_TranCorp2_RequestTime] DEFAULT (getutcdate()) NOT NULL,
    [RequestUser]  BIGINT           NOT NULL,
    [Finished]     BIT              NOT NULL,
    [FinishTime]   DATETIME         NOT NULL,
    [FinishUser]   BIGINT           NOT NULL,
    [ExpireTime]   DATETIME         NULL,
    CONSTRAINT [PK_TranCorp2] PRIMARY KEY CLUSTERED ([TranId] ASC)
);



