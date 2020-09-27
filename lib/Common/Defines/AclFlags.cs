using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CMS.Defines
{
    [Flags]
    public enum AclFlags
    {
        RootOnly = 0x01,
        CorpRootOnly = 0x02,
    }
}
