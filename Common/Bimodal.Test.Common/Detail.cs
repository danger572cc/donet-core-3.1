using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bimodal.Test.Common
{
    public sealed class Detail<T> where T : class
    {
        [JsonProperty("Status")]
        public int StatusCode { get; }

        [JsonProperty("Response")]
        public List<T> Response { get; }

        public Detail(int status, List<T> response)
        {
            StatusCode = status;
            Response = response;
        }
    }
}
