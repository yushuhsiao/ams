using System.Collections;
using System.Net;

namespace CMS
{
    public interface IApiResult
    {
        Status StatusCode { get; set; }
        string Message { get; set; }

        IEnumerable Rows { get; }
        int? RowIndex { get; set; }
        int? RowCount { get; set; }

        HttpStatusCode? HttpStatusCode { get; set; }
    }
}
