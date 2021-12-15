CREATE TABLE [dbo].[TranLog] (
    [sn]             BIGINT           IDENTITY (1, 1) NOT NULL,
    [LogType]        INT              NOT NULL,
    [CorpId]         INT              NOT NULL,
    [CorpName]       VARCHAR (20)     NOT NULL,
    [ParentId]       BIGINT           NOT NULL,
    [ParentName]     VARCHAR (20)     NOT NULL,
    [UserId]         BIGINT           NOT NULL,
    [UserName]       VARCHAR (20)     NOT NULL,
    [PlatformId]     INT              NOT NULL,
    [PlatformName]   VARCHAR (20)     NOT NULL,
    [TranId]         UNIQUEIDENTIFIER NOT NULL,
    [PaymentAccount] UNIQUEIDENTIFIER NULL,
    [SerialNumber]   VARCHAR (16)     NOT NULL,
    [PrevBalance1]   DECIMAL (19, 6)  NOT NULL,
    [Amount1]        DECIMAL (19, 6)  NOT NULL,
    [Balance1]       DECIMAL (19, 6)  NOT NULL,
    [PrevBalance2]   DECIMAL (19, 6)  NOT NULL,
    [Amount2]        DECIMAL (19, 6)  NOT NULL,
    [Balance2]       DECIMAL (19, 6)  NOT NULL,
    [PrevBalance3]   DECIMAL (19, 6)  NOT NULL,
    [Amount3]        DECIMAL (19, 6)  NOT NULL,
    [Balance3]       DECIMAL (19, 6)  NOT NULL,
    [CurrencyA]      INT              NOT NULL,
    [CurrencyB]      INT              NOT NULL,
    [CurrencyX]      DECIMAL (19, 6)  NOT NULL,
    [RequestIP]      VARCHAR (20)     NOT NULL,
    [RequestTime]    DATETIME         NOT NULL,
    [CreateTime]     DATETIME         CONSTRAINT [DF_TranLog_CreateTime] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_TranLog] PRIMARY KEY CLUSTERED ([sn] ASC)
);



