using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace GLT.Entity
{
    [TableName("Corps", Database = _Consts.Database.CoreDB)]
    public class CorpInfo : Abstract.BaseData
    {
        public int Id { get; set; }
        public SqlTimeStamp ver { get; set; }
        public string Name { get; set; }
        public int Mode { get; set; }
        public ActiveState Active { get; set; }
        public CurrencyCode Currency { get; set; }
        public string Prefix { get; set; }
    }
}
