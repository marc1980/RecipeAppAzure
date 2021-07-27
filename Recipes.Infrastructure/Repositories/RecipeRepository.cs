using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Recipes.Core.Application.Contracts;
using Recipes.Core.Domain.Entities;

namespace Recipes.Infrastructure.Repositories
{
    public class RecipeRepository : IRecipeRepository
    {
        private Container container;

        public RecipeRepository(CosmosClient dbClient, string databaseName, string containerName)
        {
            container = dbClient.GetContainer(databaseName, containerName);
        }

        public async Task AddAsync(Recipe recipe)
        {
            await container.CreateItemAsync<Recipe>(recipe, new PartitionKey(recipe.Id));
        }

        public async void UpdateAsync(Recipe recipe)
        {
            await container.UpsertItemAsync<Recipe>(recipe, new PartitionKey(recipe.Id));
        }        
        
        public async Task<Recipe> GetAsync(string recipeId)
        {
            try
            {
                ItemResponse<Recipe> response = await container.ReadItemAsync<Recipe>(recipeId, new PartitionKey(recipeId));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task<IEnumerable<Recipe>> GetItemsAsync(string queryString)
        {
            var query = container.GetItemQueryIterator<Recipe>(new QueryDefinition(queryString));
            var results = new List<Recipe>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();

                results.AddRange(response.ToList());
            }

            return results;
        }
    }
}
