CREATE TABLE [dbo].[UserAcl] (
    [AclId]  INT    NOT NULL,
    [UserId] BIGINT NOT NULL,
    [Flags]  INT    NULL,
    CONSTRAINT [PK_UserAcl] PRIMARY KEY CLUSTERED ([UserId] ASC, [AclId] ASC)
);

