using System.IO;
using System.Net;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Recipes.Core.Application.Features.Recipes.Commands.CreateRecipe;
using Recipes.Core.Application.Features.Recipes.Commands.UpdateRecipe;

namespace Recipes.Api
{
    public class RecipeFunctions
    {
        private readonly IMediator mediator;
        private readonly ILogger<RecipeFunctions> logger;
        private readonly IFunctionExecutor functionExecutor;

        public RecipeFunctions(IMediator mediator, ILogger<RecipeFunctions> logger, IFunctionExecutor functionExecutor)
        {
            this.mediator = mediator;
            this.logger = logger;
            this.functionExecutor = functionExecutor;
        }

        [Function("AddRecipe")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "recipes")]
            HttpRequestData req)
        {
            string requestBody = new StreamReader(req.Body).ReadToEnd();
            var command = JsonConvert.DeserializeObject<CreateRecipeCommand>(requestBody);


            return await functionExecutor.ExecuteAsync(req, async () =>
            {
                return await mediator.Send(command);
            });

        }

        [Function("UpdateRecipe")]
        public HttpResponseData Run(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "recipes/{id}")]
            HttpRequestData req,
            string id)
        {
            string requestBody = new StreamReader(req.Body).ReadToEnd();
            var command = JsonConvert.DeserializeObject<UpdateRecipeCommand>(requestBody);

            var response = req.CreateResponse();
            response.Headers.Add("Content-Type", "application/json");
            
            if (command.Id != id) 
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                return response;
            }

            var succes = mediator.Send(command).Result;
                
            return response;
        }
    }
}
