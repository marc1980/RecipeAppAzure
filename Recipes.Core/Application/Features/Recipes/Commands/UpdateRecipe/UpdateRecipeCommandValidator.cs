using FluentValidation;

namespace Recipes.Core.Application.Features.Recipes.Commands.UpdateRecipe
{
    public class UpdateRecipeCommandValidator : AbstractValidator<UpdateRecipeCommand>
    {
        public UpdateRecipeCommandValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull();
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull()
                .MaximumLength(35).WithMessage("{PropertyName} has a maximum lenght of 35 characters");
            RuleFor(c => c.ShortDescription)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull()
                .MaximumLength(45).WithMessage("{PropertyName} has a maximum lenght of 45 characters");
            RuleFor(c => c.Description)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull()
                .MaximumLength(120).WithMessage("{PropertyName} has a maximum lenght of 120 characters");

            RuleFor(c => c.Ingredients)
                .NotEmpty().WithMessage("{PropertyName} can not be empty")
                .NotNull();
            RuleForEach(c => c.Ingredients).ChildRules(ingredient => {
                ingredient.RuleFor(i => i.Name)
                    .NotEmpty().WithMessage("{CollectionIndex} {PropertyName} can not be empty")
                    .NotNull()
                    .MaximumLength(100).WithMessage("{CollectionIndex} {PropertyName} has a maximum lenght of 100 characters");
                ingredient.RuleFor(i => i.Amount)
                    .NotEmpty().WithMessage("{CollectionIndex} {PropertyName} can not be empty")
                    .NotNull()
                    .InclusiveBetween(1, 999).WithMessage("{CollectionIndex} {PropertyName} must be between 1 and 999");
            });

            RuleFor(c => c.PreparationSteps)
                .NotEmpty().WithMessage("{PropertyName} can not be empty")
                .NotNull();
            RuleForEach(c => c.PreparationSteps).ChildRules(step => {
                step.RuleFor(s => s.Description)
                    .NotEmpty().WithMessage("{CollectionIndex} {PropertyName} can not be empty")
                    .NotNull()
                    .MaximumLength(250).WithMessage("{CollectionIndex} {PropertyName} has a maximum lenght of 250 characters");
                step.RuleFor(s => s.Rank)
                    .NotEmpty().WithMessage("{CollectionIndex} {PropertyName} can not be empty")
                    .NotNull()
                    .InclusiveBetween(1, 999).WithMessage("{CollectionIndex} {PropertyName} must be between 1 and 999");
            });
        }
    }
}
