using System.IO;
using Microsoft.Azure.Functions.Worker.Http;
using Newtonsoft.Json;

namespace Recipes.Api
{
    static class HttpRequestDataExtension
    {
        public static T GetCommand<T>(this HttpRequestData req)
        {
            string requestBody = new StreamReader(req.Body).ReadToEnd();
            var command = JsonConvert.DeserializeObject<T>(requestBody);
            return command;
        }

        public static int GetFromQueryParametersOrDefault(this HttpRequestData req, string key, int defaultValue)
        {
            var query = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
            var pageString = query.Get(key);
            return pageString != null ? int.Parse(pageString) : defaultValue;
        }
    }
}
