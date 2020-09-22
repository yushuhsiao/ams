
using System.Net;

namespace CMS
{
    [JsonHelper.StringEnum(false)]
    public enum Status : int
    {
        /// <summary>
        ///   未知錯誤
        /// </summary>
        Exception = -1,

        /// <summary>
        /// 未知
        /// </summary>
        Unknown = 0,

        Success = 1,

        CorpNotExist = 2,
        CorpAlreadyExist = 3,
    }
}