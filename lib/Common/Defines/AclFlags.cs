using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CMS
{
    [Flags]
    public enum AclFlags : sbyte
    {
        RootOnly = 0x01,
        CorpRootOnly = 0x02,
    }
}
