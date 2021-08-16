using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker.Http;
using Recipes.Core.Application.Exceptions;

namespace Recipes.Api
{
    public class FunctionExecutor : IFunctionExecutor
    {
        public async Task<HttpResponseData> ExecuteAsync(
            HttpRequestData req,
            Func<Task<HttpResponseData>> func)
        {
            var response = req.CreateResponse();
            response.Headers.Add("Content-Type", "application/json");

            try
            {
                response.StatusCode = HttpStatusCode.OK;
                var result = await func();
                await response.WriteAsJsonAsync(result);
            }
            catch (ValidationException ve)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                await response.WriteAsJsonAsync(ve.ValidationErrors);
            }
            catch (NotFoundException)
            {
                response.StatusCode = HttpStatusCode.NotFound;
            }

            return response;
        }
    }
}
