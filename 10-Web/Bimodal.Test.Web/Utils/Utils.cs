using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bimodal.Test.Web.Utils
{
    public static class Utils
    {
        public static Dictionary<string, TValue> ToDictionary<TValue>(this object obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, TValue>>(json, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            return dictionary;
        }

        public static T ToObject<T>(this object obj) 
        {
            var json = JsonConvert.SerializeObject(obj);
            var objectCasted = JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings {  NullValueHandling = NullValueHandling.Ignore });
            return objectCasted;
        }
    }
}
