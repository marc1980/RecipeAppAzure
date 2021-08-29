using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Recipes.Core.Application.Contracts;

namespace Recipes.Core.Application.Features.Recipes.Queries.GetRecipesList
{
    public class GetRecipeListQueryHandler : IRequestHandler<GetRecipeListQuery, IEnumerable<RecipeSummaryDto>>
    {
        private readonly IRecipeRepository recipeRepository;
        private readonly ILogger<GetRecipeListQueryHandler> logger;

        public GetRecipeListQueryHandler(IRecipeRepository recipeRepository, ILogger<GetRecipeListQueryHandler> logger)
        {
            this.recipeRepository = recipeRepository;
            this.logger = logger;
        }

        public async Task<IEnumerable<RecipeSummaryDto>> Handle(GetRecipeListQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await recipeRepository.GetItemsAsync(request.Page, request.PageSize);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
