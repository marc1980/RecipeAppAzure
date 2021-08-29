using MediatR;
using Recipes.Core.Domain.Entities;

namespace Recipes.Core.Application.Features.Recipes.Queries.GetRecipeDetails
{
    public class GetRecipeDetailsQuery : IRequest<Recipe>
    {
        public string Id { get; set; }
    }
}
