CREATE TABLE [dbo].[UserAclDelegate] (
    [CorpId] INT    NOT NULL,
    [AclId]  INT    NOT NULL,
    [UserId] BIGINT NOT NULL,
    CONSTRAINT [PK_UserAclDelegate] PRIMARY KEY CLUSTERED ([CorpId] ASC, [AclId] ASC)
);

