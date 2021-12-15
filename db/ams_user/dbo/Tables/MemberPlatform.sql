CREATE TABLE [dbo].[MemberPlatform] (
    [MemberId]   BIGINT NOT NULL,
    [PlatformId] INT    NOT NULL,
    CONSTRAINT [PK_MemberPlatform] PRIMARY KEY CLUSTERED ([MemberId] ASC, [PlatformId] ASC)
);

