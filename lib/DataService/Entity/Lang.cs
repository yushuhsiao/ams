// 多國語系定義

namespace CMS.Entity
{
    public class Lang
    {
        /// <summary>
        /// 分類
        /// </summary>
        public string Category { get; set; }
        
        /// <summary>
        /// 項目名稱
        /// </summary>
        public string Name { get; set; }
        
        public int LCID { get; set; }
        
        public string Text { get; set; }
    }
}