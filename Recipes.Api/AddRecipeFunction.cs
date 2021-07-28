using System.IO;
using System.Net;
using FluentValidation;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Recipes.Core.Application.Features.Recipes.Commands.CreateRecipe;

namespace Recipes.Api
{
    public class AddRecipeFunction
    {
        private readonly IMediator mediator;
        private readonly ILogger<AddRecipeFunction> logger;

        public AddRecipeFunction(IMediator mediator, ILogger<AddRecipeFunction> logger)
        {
            this.mediator = mediator;
            this.logger = logger;
        }

        [Function("AddRecipe")]
        public HttpResponseData Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post")]
            HttpRequestData req)
        {
            string requestBody = new StreamReader(req.Body).ReadToEnd();
            var command = JsonConvert.DeserializeObject<CreateRecipeCommand>(requestBody);

            var response = req.CreateResponse();
            response.Headers.Add("Content-Type", "application/json");

            try
            {
                var succes = mediator.Send(command).Result;
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (ValidationException validationException)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.WriteAsJsonAsync(JsonConvert.SerializeObject(validationException.Errors));
            };
                
            return response;
        }
    }
}
