using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Recipes.Core.Application.Contracts;
using Recipes.Core.Application.Exceptions;

namespace Recipes.Core.Application.Features.Recipes.Commands.AddReview
{
    public class AddReviewCommandValidator : AbstractValidator<AddReviewCommand>
    {
        private readonly IRecipeRepository recipeRepository;

        public AddReviewCommandValidator(IRecipeRepository recipeRepository)
        {
            this.recipeRepository = recipeRepository;

            RuleFor(c => c.RecipeId)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull();
            RuleFor(c => c.Reviewer)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull()
                .MaximumLength(100).WithMessage("{PropertyName} has a maximum lenght of 35 characters");
            RuleFor(c => c.Body)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull()
                .MaximumLength(2000).WithMessage("{PropertyName} has a maximum lenght of 45 characters");
            RuleFor(c => c)
                .MustAsync(RecipeMustExist).WithMessage("Recipe does not exist");
        }

        private async Task<bool> RecipeMustExist(AddReviewCommand review, CancellationToken cancellationToken)
        {
            try
            {
                return await recipeRepository.GetAsync(review.RecipeId) != null;
            }
            catch (RecipeNotFoundInRepositoryException)
            {
                throw new NotFoundApiException(nameof(review.RecipeId), review.RecipeId);
            }

        }
    }
}
