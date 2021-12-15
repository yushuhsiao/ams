CREATE TABLE [dbo].[LoginLog] (
    [sn]            BIGINT       IDENTITY (1, 1) NOT NULL,
    [CorpId]        INT          NULL,
    [UserId]        BIGINT       NULL,
    [LoginType]     VARCHAR (10) NOT NULL,
    [CorpName]      VARCHAR (20) NOT NULL,
    [UserName]      VARCHAR (20) NOT NULL,
    [Password]      VARCHAR (50) NULL,
    [IP]            VARCHAR (50) NULL,
    [Result]        VARCHAR (30) NOT NULL,
    [ResultMessage] VARCHAR (50) NULL,
    [LoginTime]     DATETIME     CONSTRAINT [DF_LoginLog_LoginTime] DEFAULT (getutcdate()) NOT NULL,
    [CreateTime]    DATETIME     CONSTRAINT [DF_LoginLog_CreateTime] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_LoginLog] PRIMARY KEY CLUSTERED ([sn] ASC)
);



