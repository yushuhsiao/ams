CREATE TABLE [dbo].[GameLog] (
    [sn]           BIGINT          NOT NULL,
    [CorpID]       INT             NOT NULL,
    [CorpName]     VARCHAR (20)    NOT NULL,
    [ParentID]     INT             NOT NULL,
    [ParentName]   VARCHAR (20)    NOT NULL,
    [UserID]       INT             NOT NULL,
    [UserName]     VARCHAR (20)    NOT NULL,
    [PlatformID]   INT             NOT NULL,
    [PlatformName] VARCHAR (20)    NOT NULL,
    [PlatformType] INT             NOT NULL,
    [GameClass]    INT             NOT NULL,
    [GameID]       INT             NOT NULL,
    [GameName]     VARCHAR (50)    NOT NULL,
    [Amount]       DECIMAL (19, 6) NOT NULL,
    [Balance]      DECIMAL (19, 6) NOT NULL,
    [CurrencyA]    SMALLINT        NOT NULL,
    [CurrencyB]    SMALLINT        NOT NULL,
    [CurrencyX]    DECIMAL (19, 6) NOT NULL,
    [CreateTime]   DATETIME        CONSTRAINT [DF_GameLog_CreateTime] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_GameLog] PRIMARY KEY CLUSTERED ([sn] ASC)
);



