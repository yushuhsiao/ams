using System;

namespace CMS
{
    [Flags]
    public enum UserType : sbyte
    {
        General = 0,    // 代理/會員
        Admin = 1,      // 管理帳號
        Temporary = 2,  // 散客
        //Auto = 0x00,
        //Guest = 0x00,
        //Agent = 0x40,
        //Admin = 0x20,
        //Member = 0x10,
    }
}