using Newtonsoft.Json;
using System.Collections;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace GLT
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class ApiResult : ApiResult<object>
    {
        //private static JsonResult DefaultResult = new JsonResult(null) { StatusCode = (int)System.Net.HttpStatusCode.BadRequest };
        private static ApiResult _Empty = new ApiResult() { StatusCode = Status.Success };
        private static ApiResult _BadRequest = new ApiResult() { HttpStatusCode = System.Net.HttpStatusCode.BadRequest };

        private static JsonResult BuildResult(IApiResult result)
            => new JsonResult(result) { StatusCode = (int?)result.HttpStatusCode };

        public static IActionResult FromException(Exception ex)
        {
            if (ex is IApiResult api_result)
            {
                api_result.Message = api_result.Message ?? ex.Message;
            }
            else
            {
                api_result = new ApiResult()
                {
                    StatusCode = Status.Exception,
                    Message = ex.Message,
                    HttpStatusCode = System.Net.HttpStatusCode.InternalServerError
                };
            }
            if (ex is ArgumentNullException)
                api_result.HttpStatusCode = System.Net.HttpStatusCode.BadRequest;

            return BuildResult(api_result);
        }

        public static IActionResult FromActionResult(IActionResult data)
        {
            ApiResult result = null;
            if (data == null)
                result = _Empty;
            else if (data is EmptyResult)
                result = _Empty;
            else if (data is ObjectResult r1)
            {
                result = new ApiResult()
                {
                    Data = r1.Value,
                    StatusCode = Status.Success,
                    HttpStatusCode = (System.Net.HttpStatusCode?)r1.StatusCode,
                };
            }
            return BuildResult(result ?? _BadRequest);
        }
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class ApiResult<T> : IApiResult
    {
        [JsonProperty(_Consts.Api.StatusCode)]
        public Status StatusCode { get; set; }

        private string _message;

        [JsonProperty(_Consts.Api.Message)]
        public string Message
        {
            get => _message ?? StatusCode.ToString();
            set => _message = value;
        }

        public T Data { get; set; }

        public HttpStatusCode? HttpStatusCode { get; set; }

        public IEnumerable<T> Rows { get; set; }

        [JsonProperty(_Consts.Api.Rows)]
        IEnumerable IApiResult.Rows
        {
            get
            {
                if (this.Rows != null)
                {
                    foreach (var d in this.Rows)
                        yield return d;
                }
                else if (this.Data != null)
                    yield return this.Data;
            }
        }

        [JsonProperty(_Consts.Api.RowIndex, NullValueHandling = NullValueHandling.Ignore)]
        public int? RowIndex { get; set; }

        [JsonProperty(_Consts.Api.RowCount, NullValueHandling = NullValueHandling.Ignore)]
        public int? RowCount { get; set; }
    }
}
