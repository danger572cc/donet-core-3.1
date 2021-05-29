using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bimodal.Test.Common
{
    public sealed class Detail
    {
        [JsonProperty("Status")]
        public int StatusCode { get; }

        [JsonProperty("Response")]
        
        public object Response { get; }

        public Detail(int status, object response)
        {
            StatusCode = status;
            Response = response;
        }

        public Detail(int status) 
        {
            StatusCode = status;
            Response = null;
        }
    }
}
