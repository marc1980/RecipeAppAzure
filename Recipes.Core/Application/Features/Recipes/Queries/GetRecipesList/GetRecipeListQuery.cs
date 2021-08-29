using System.Collections.Generic;
using MediatR;

namespace Recipes.Core.Application.Features.Recipes.Queries.GetRecipesList
{
    public class GetRecipeListQuery : IRequest<IEnumerable<RecipeSummaryDto>> 
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
