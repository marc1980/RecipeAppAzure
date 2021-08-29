using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Recipes.Core.Application.Features.Recipes.Commands.AddReview;
using Recipes.Core.Application.Features.Recipes.Commands.CreateRecipe;
using Recipes.Core.Application.Features.Recipes.Commands.UpdateRecipe;
using Recipes.Core.Application.Features.Recipes.Queries.GetRecipeDetails;
using Recipes.Core.Application.Features.Recipes.Queries.GetRecipesList;

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
        public async Task<HttpResponseData> AddRecipe(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "recipes")]
            HttpRequestData req)
        {
            var command = req.GetCommand<CreateRecipeCommand>();
            return await functionExecutor.ExecuteAsync(req, async () => await mediator.Send(command));

        }

        [Function("UpdateRecipe")]
        public async Task<HttpResponseData> UpdateRecipe(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "recipes/{id}")]
            HttpRequestData req,
            string id)
        {
            var command = req.GetCommand<UpdateRecipeCommand>();
            return await functionExecutor.ExecuteAsync(req, async () => await mediator.Send(command));
        }

        [Function("GetRecipe")]
        public async Task<HttpResponseData> GetRecipe(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "recipes/{id}")]
            HttpRequestData req,
            string id)
        {
            return await functionExecutor.ExecuteAsync(req, async () => await mediator.Send(new GetRecipeDetailsQuery { Id = id }));
        }

        [Function("GetRecipeList")]
        public async Task<HttpResponseData> GetRecipeList(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "recipes")]
            HttpRequestData req)
        {
            var page = req.GetFromQueryParametersOrDefault("page", 1);
            var pageSize = req.GetFromQueryParametersOrDefault("pageSize", 10);

            return await functionExecutor.ExecuteAsync(req, async () => await mediator.Send(new GetRecipeListQuery() { Page = page, PageSize = pageSize }));
        }

        [Function("AddReview")]
        public async Task<HttpResponseData> AddReview(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "recipes/{id}/review")]
            HttpRequestData req,
            string id)
        {
            var command = req.GetCommand<AddReviewCommand>();
            return await functionExecutor.ExecuteAsync(req, async () => await mediator.Send(command));
        }
    }
}
