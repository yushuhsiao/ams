using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace GLT.Entity
{
    [TableName("Confiig", Database = _Consts.db.CoreDB)]
    public class Config
    {
        public int CorpId { get; set; }
        public string Key1 { get; set; }
        public string Key2 { get; set; }
        public string Value { get; set; }
    }
}
