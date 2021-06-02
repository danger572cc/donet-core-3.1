using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bimodal.Test.Web.Utils
{
    public static class Utils
    {
        public static string GetLocalUrl(this IUrlHelper urlHelper, string localUrl)
        {
            if (!urlHelper.IsLocalUrl(localUrl))
            {
                return urlHelper.Page("/Index");
            }
            return localUrl;
        }

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
