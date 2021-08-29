using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Recipes.Core.Application.Contracts;
using Recipes.Core.Application.Exceptions;
using Recipes.Core.Domain.Entities;

namespace Recipes.Core.Application.Features.Recipes.Queries.GetRecipeDetails
{
    public class GetRecipeDetailsQueryHandler : IRequestHandler<GetRecipeDetailsQuery, Recipe>
    {
        private readonly IRecipeRepository recipeRepository;
        private readonly ILogger<GetRecipeDetailsQueryHandler> logger;

        public GetRecipeDetailsQueryHandler(IRecipeRepository recipeRepository, ILogger<GetRecipeDetailsQueryHandler> logger)
        {
            this.recipeRepository = recipeRepository;
            this.logger = logger;
        }

        public async Task<Recipe> Handle(GetRecipeDetailsQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Id))
            {
                throw new NotFoundApiException(nameof(Recipe), request.Id);
            }

            try
            {
                return await recipeRepository.GetAsync(request.Id);
            }
            catch (RecipeNotFoundInRepositoryException ex)
            {
                logger.LogError(ex.Message);
                throw new NotFoundApiException(nameof(Recipe), request.Id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
