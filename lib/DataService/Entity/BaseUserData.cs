using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CMS.Entity
{
    public class GeneralUser : Abstract.BaseUserData
    {
        public Abstract.GeneralUserData ExtData;
    }

    public class AdminUser : Abstract.BaseUserData
    {
        public Abstract.AdminUserData ExtData;
    }
}
namespace CMS.Entity.Abstract
{
    [TableName("Users", Database = _Consts.Database.UserDB)]
    public class BaseUserData : BaseData
    {
        public int Id { get; set; }
        public SqlTimeStamp ver { get; set; }
        public UserType UserType { get; set; }
        public int CorpId { get; set; }
        public string Name { get; set; }
        public ActiveState Active { get; set; }
        public int ParentId { get; set; }
        public string DisplayName { get; set; }
        public int Depth { get; set; }
    }

    /// <summary>
    /// 會員或代理資料
    /// </summary>
    [TableName("Users_General", Database = _Consts.Database.UserDB)]
    public class GeneralUserData
    {
        public int Id { get; set; }

        /// <summary>
        /// 最多可以有幾個會員或代理
        /// 0 : 帳號只能是會員
        /// null : 不限制
        /// </summary>
        public int MaxUsers { get; set; }

        /// <summary>
        /// 會員或代理最多可以有幾層
        /// null : 不限制
        /// </summary>
        public int MaxDepth { get; set; }

        /// <summary>
        /// 最多可以有幾個子帳號, UserType = 0 的時候才有用
        /// null : 不限制
        /// </summary>
        public int MaxAdmins { get; set; }
    }

    /// <summary>
    /// 員工資料
    /// </summary>
    [TableName("Users_Admin", Database = _Consts.Database.UserDB)]
    public class AdminUserData
    {
        public int Id { get; set; }

        /// <summary>
        /// 主管
        /// </summary>
        public int ManagerId { get; set; }

        // 員工識別碼
        // 員工類型
        // 部門
    }
}
