using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Recipes.Core.Application.Contracts;
using Recipes.Core.Domain.Entities;
using Recipes.Core.Application.Exceptions;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;

namespace Recipes.Core.Application.Features.Recipes.Commands.UpdateRecipe
{
    public class UpdateRecipeCommandHandler : IRequestHandler<UpdateRecipeCommand, bool>
    {
        private readonly IRecipeRepository recipeRepository;
        private readonly IMapper mapper;
        private readonly ILogger<UpdateRecipeCommandHandler> logger;

        public UpdateRecipeCommandHandler(IRecipeRepository recipeRepository, IMapper mapper, ILogger<UpdateRecipeCommandHandler> logger)
        {
            this.recipeRepository = recipeRepository;
            this.mapper = mapper;
            this.logger = logger;
        }
        public async Task<bool> Handle(UpdateRecipeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var recipe = await recipeRepository.GetAsync(request.Id);

                var validator = new UpdateRecipeCommandValidator();
                var validationResult = await validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                {
                    logger.LogError("validation failed");
                    throw new ValidationApiException(validationResult);
                }

                mapper.Map(request, recipe, typeof(UpdateRecipeCommand), typeof(Recipe));

                await recipeRepository.UpdateAsync(recipe);

                return true;
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
