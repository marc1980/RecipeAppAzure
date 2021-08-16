using System.Collections.Generic;
using System.Threading.Tasks;
using Recipes.Core.Domain.Entities;

namespace Recipes.Core.Application.Contracts
{
    public interface IRecipeRepository
    {
        Task<Recipe> AddAsync(Recipe recipe);
        Task UpdateAsync(Recipe recipe);
        Task<Recipe> GetAsync(string recipeId);
        Task<IEnumerable<Recipe>> GetItemsAsync(string queryString);
    }
}
