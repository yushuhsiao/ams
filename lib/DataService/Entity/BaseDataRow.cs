using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Entity.Abstract
{
    public class BaseData
    {
        public DateTime CreateTime { get; set; }
        public int CreateUser { get; set; }
        public DateTime ModifyTime { get; set; }
        public int ModifyUser { get; set; }
    }
}
