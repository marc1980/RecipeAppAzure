using System;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker.Http;

namespace Recipes.Api
{
    public interface IFunctionExecutor
    {
        public Task<HttpResponseData> ExecuteAsync<T>(
            HttpRequestData req,
            Func<Task<T>> func);
    }
}