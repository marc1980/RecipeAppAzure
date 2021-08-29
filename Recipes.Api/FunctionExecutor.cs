using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker.Http;
using Recipes.Core.Application.Exceptions;

namespace Recipes.Api
{
    public class FunctionExecutor : IFunctionExecutor
    {
        public async Task<HttpResponseData> ExecuteAsync<T>(
            HttpRequestData req,
            Func<Task<T>> func)
        {
            var response = req.CreateResponse();

            try
            {
                response.StatusCode = HttpStatusCode.OK;
                var result = await func();
                await response.WriteAsJsonAsync(result);
            }
            catch (ValidationApiException ve)
            {
                await response.WriteAsJsonAsync(ve.ValidationErrors);
                response.StatusCode = HttpStatusCode.BadRequest;
            }
            catch (NotFoundApiException)
            {
                response.StatusCode = HttpStatusCode.NotFound;
            }

            return response;
        }
    }
}
