CREATE TABLE [dbo].[TableVer] (
    [_name]  VARCHAR (30) NOT NULL,
    [_index] INT          NOT NULL,
    [_ver]   ROWVERSION   NOT NULL,
    [_time]  DATETIME     CONSTRAINT [DF_TableVer__time] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_TableVer] PRIMARY KEY CLUSTERED ([_name] ASC, [_index] ASC)
);



