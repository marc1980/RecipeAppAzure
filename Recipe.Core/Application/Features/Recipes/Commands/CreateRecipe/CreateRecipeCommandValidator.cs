using FluentValidation;

namespace Recipes.Core.Application.Features.Recipes.Commands.CreateRecipe
{
    public class CreateRecipeCommandValidator : AbstractValidator<CreateRecipeCommand>
    {
        public CreateRecipeCommandValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull()
                .MaximumLength(35).WithMessage("{PropertyName} has a maximum lenght of 35 characters");
        }
    }
}
