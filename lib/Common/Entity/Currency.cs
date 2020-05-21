// 幣別轉換

using System;
using System.Data;

namespace GLT.Entity
{
    public abstract class Currency
    {
        public CurrencyCode A { get; set; }
        
        public CurrencyCode B { get; set; }
        
        public SqlTimeStamp ver { get; set; }

        /// <summary>
        /// 匯率
        /// </summary>
        public decimal X { get; set; }
        
        public DateTime ModifyTime { get; set; }
        
        public int ModifyUser { get; set; }
    }
}