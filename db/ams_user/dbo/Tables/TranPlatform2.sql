CREATE TABLE [dbo].[TranPlatform2] (
    [TranId]          UNIQUEIDENTIFIER NOT NULL,
    [ver]             BIGINT           NOT NULL,
    [LogType]         INT              NOT NULL,
    [SerialNumber]    VARCHAR (16)     NOT NULL,
    [CorpId]          INT              NOT NULL,
    [CorpName]        VARCHAR (20)     NOT NULL,
    [UserId]          BIGINT           NOT NULL,
    [UserName]        VARCHAR (20)     NOT NULL,
    [PlatformId]      INT              NOT NULL,
    [PlatformName]    VARCHAR (20)     NOT NULL,
    [PlatformBalance] DECIMAL (19, 6)  NULL,
    [Amount1]         DECIMAL (19, 6)  NOT NULL,
    [CurrencyA]       SMALLINT         NOT NULL,
    [CurrencyB]       SMALLINT         NOT NULL,
    [CurrencyX]       DECIMAL (19, 6)  NOT NULL,
    [RequestIP]       VARCHAR (20)     NOT NULL,
    [RequestTime]     DATETIME         CONSTRAINT [DF_TranPlatform2_RequestTime] DEFAULT (getutcdate()) NOT NULL,
    [RequestUser]     BIGINT           NOT NULL,
    [Busy]            DATETIME         NULL,
    [AcceptTime]      DATETIME         NOT NULL,
    [AcceptUser]      BIGINT           NOT NULL,
    [Finished]        BIT              NOT NULL,
    [FinishTime]      DATETIME         NOT NULL,
    [FinishUser]      BIGINT           NOT NULL,
    [ExpireTime]      DATETIME         NULL,
    CONSTRAINT [PK_TranPlatform2] PRIMARY KEY CLUSTERED ([TranId] ASC)
);



