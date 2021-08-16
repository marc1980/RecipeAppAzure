using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Recipes.Core.Application.Contracts;
using Recipes.Core.Domain.Entities;

namespace Recipes.Core.Application.Features.Recipes.Commands.CreateRecipe
{
    public class CreateRecipeCommandHandler : IRequestHandler<CreateRecipeCommand, Recipe>
    {
        private readonly IRecipeRepository recipeRepository;

        public CreateRecipeCommandHandler(IRecipeRepository recipeRepository)
        {
            this.recipeRepository = recipeRepository;
        }

        public async Task<Recipe> Handle(CreateRecipeCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateRecipeCommandValidator();
            await validator.ValidateAndThrowAsync(request, cancellationToken);

            //if (!validationResult.IsValid)
            //{
            //    throw new Exceptions.ValidationException(validationResult);
            //}

            return await recipeRepository.AddAsync(new Recipe()
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.Name,

            });
        }
    }
}
