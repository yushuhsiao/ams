using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CMS.Entity
{
    [TableName("Config", Database = _Consts.Database.CoreDB)]
    public class Config
    {
        internal string Key;

        public int Id { get; set; }
        public int CorpId { get; set; }
        public string Key1 { get; set; }
        public string Key2 { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
    }
}
