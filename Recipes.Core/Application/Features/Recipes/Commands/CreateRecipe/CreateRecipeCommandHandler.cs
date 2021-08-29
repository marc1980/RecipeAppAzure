using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Recipes.Core.Application.Contracts;
using Recipes.Core.Domain.Entities;
using AutoMapper;
using Recipes.Core.Application.Exceptions;
using Microsoft.Extensions.Logging;

namespace Recipes.Core.Application.Features.Recipes.Commands.CreateRecipe
{
    public class CreateRecipeCommandHandler : IRequestHandler<CreateRecipeCommand, Recipe>
    {
        private readonly IRecipeRepository recipeRepository;
        private readonly IMapper mapper;
        private readonly ILogger<CreateRecipeCommandHandler> logger;

        public CreateRecipeCommandHandler(IRecipeRepository recipeRepository, IMapper mapper, ILogger<CreateRecipeCommandHandler> logger)
        {
            this.recipeRepository = recipeRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<Recipe> Handle(CreateRecipeCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateRecipeCommandValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationApiException(validationResult);
            }

            var recipe = mapper.Map<Recipe>(request);
            recipe.Id = Guid.NewGuid().ToString();

            try
            {
                return await recipeRepository.AddAsync(recipe);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
