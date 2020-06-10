using Newtonsoft.Json;
using System;
using System.Collections;
using System.Net;

namespace GLT
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class ApiException : Exception, IApiResult
    {
        public ApiException(Status status, string message = null) : base(message) { }

        [JsonProperty(_Consts.Api.StatusCode)]
        public Status StatusCode { get; set; }

        private string _message;

        [JsonProperty(_Consts.Api.Message)]
        string IApiResult.Message
        {
            get => _message ?? base.Message ?? StatusCode.ToString();
            set => _message = value;
        }

        IEnumerable IApiResult.Rows { get; }

        int? IApiResult.RowIndex { get; set; }

        int? IApiResult.RowCount { get; set; }

        HttpStatusCode? IApiResult.HttpStatusCode { get; set; }
    }
}