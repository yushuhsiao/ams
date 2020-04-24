CREATE TABLE [dbo].[TableVer] (
    [_db]    VARCHAR (20) NOT NULL,
    [_table] VARCHAR (30) NOT NULL,
    [_index] INT          NOT NULL,
    [_ver]   ROWVERSION   NOT NULL,
    [_time]  DATETIME     CONSTRAINT [DF_Table_1__time] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_TableVer] PRIMARY KEY CLUSTERED ([_db] ASC, [_table] ASC, [_index] ASC)
);

