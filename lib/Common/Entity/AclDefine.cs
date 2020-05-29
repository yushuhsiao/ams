using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace GLT.Entity
{
    [TableName("AclDefine", Database = _Consts.db.CoreDB)]
    public class AclDefine
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
    }
}
