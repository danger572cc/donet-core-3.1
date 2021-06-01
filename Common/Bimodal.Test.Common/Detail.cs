using Newtonsoft.Json;

namespace Bimodal.Test.Common
{
    public sealed class Detail
    {
        [JsonProperty("Status")]
        public int StatusCode { get; set; }

        [JsonProperty("Response")]
        
        public object Response { get; set; }

        public Detail() { }

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
