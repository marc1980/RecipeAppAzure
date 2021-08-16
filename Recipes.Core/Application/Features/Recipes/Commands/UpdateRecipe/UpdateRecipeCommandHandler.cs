using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Recipes.Core.Application.Contracts;
using Recipes.Core.Domain.Entities;
using Recipes.Core.Application.Exceptions;

namespace Recipes.Core.Application.Features.Recipes.Commands.UpdateRecipe
{
    public class UpdateRecipeCommandHandler : IRequestHandler<UpdateRecipeCommand, bool>
    {
        private readonly IRecipeRepository recipeRepository;

        public UpdateRecipeCommandHandler(IRecipeRepository recipeRepository)
        {
            this.recipeRepository = recipeRepository;
        }
        public async Task<bool> Handle(UpdateRecipeCommand request, CancellationToken cancellationToken)
        {
            var recipe = await recipeRepository.GetAsync(request.Id);

            if (recipe == null) 
            {
                throw new NotFoundException(nameof(Recipe), request.Id);
            }

            var validator = new UpdateRecipeCommandValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult);
            }

            await recipeRepository.UpdateAsync(new Recipe()
            {
                Id = request.Id,
                Name = request.Name,

            });

            return true;
        }
    }
}
