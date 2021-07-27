using System.Collections.Generic;
using System.Threading.Tasks;
using Recipes.Core.Domain.Entities;

namespace Recipes.Core.Application.Contracts
{
    public interface IRecipeRepository
    {
        Task AddAsync(Recipe recipe);
        void UpdateAsync(Recipe recipe);
        Task<Recipe> GetAsync(string recipeId);
        Task<IEnumerable<Recipe>> GetItemsAsync(string queryString);
    }
}
