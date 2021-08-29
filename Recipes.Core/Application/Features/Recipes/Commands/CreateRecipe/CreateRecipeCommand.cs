using System.Collections.Generic;
using MediatR;
using Recipes.Core.Domain.Entities;

namespace Recipes.Core.Application.Features.Recipes.Commands.CreateRecipe
{
    public class CreateRecipeCommand : IRequest<Recipe>
    {
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public int PreparationTime { get; set; }
        public int Portions { get; set; }
        public IEnumerable<Ingredient> Ingredients { get; set; }
        public IEnumerable<PreparationStep> PreparationSteps { get; set; }
    }
}
