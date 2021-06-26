using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CMS.Entity
{
    /// <summary>
    /// 權限項目定義
    /// </summary>
    [TableName("AclDefine", Database = _Consts.Database.CoreDB)]
    public class AclDefine
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public AclFlags Flags { get; set; }
    }

    [TableName("AclGroup", Database = _Consts.Database.UserDB)]
    public class AclGroup
    {
        public int GroupId { get; set; }
        public string Name { get; set; }
        public int OwnerUserId { get; set; }
    }

    [TableName("AclGroupRole", Database = _Consts.Database.UserDB)]
    public class AclGroupRole
    {
        public int GroupId { get; set; }
        public int AclId { get; set; }
    }

    [TableName("AclGroupUser", Database = _Consts.Database.UserDB)]
    public class AclGroupUser
    {
        public int GroupId { get; set; }
        public int UserId { get; set; }
    }

    [TableName("AclUserRole", Database = _Consts.Database.UserDB)]
    public class AclUserRole
    {
        public int AclId { get; set; }
        public int UserId { get; set; }
    }
}
