using System;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker.Http;

namespace Recipes.Api
{
    public interface IFunctionExecutor
    {
        public Task<HttpResponseData> ExecuteAsync(
            HttpRequestData req,
            Func<Task<HttpResponseData>> func);
    }
}