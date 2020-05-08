using System;
using System.Collections.Generic;
using System.Text;

namespace GLT
{
    /// <summary>
    /// 交易類型, 奇數為提領, 偶數為存款
    /// </summary>
    public enum LogType
    {
        /// <summary>
        /// 提領
        /// </summary>
        Withdrawal = 1,

        /// <summary>
        /// 存款
        /// </summary>
        Deposit = 2,
    }
}
