using System;

namespace GLT
{
    [Flags]
    public enum UserType : sbyte
    {
        //Auto = 0x00,
        //Guest = 0x00,
        Agent = 0x40,
        Admin = 0x20,
        Member = 0x10,
    }
}