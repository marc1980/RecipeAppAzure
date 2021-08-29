using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Recipes.Core.Application.Contracts;
using Recipes.Core.Domain.Entities;
using Recipes.Core.Application.Features.Recipes.Queries.GetRecipesList;
using Recipes.Core.Application.Exceptions;

namespace Recipes.Infrastructure.Repositories
{
    public class RecipeRepository : IRecipeRepository
    {
        private Container container;

        public RecipeRepository(CosmosClient dbClient, string databaseName, string containerName)
        {
            container = dbClient.GetContainer(databaseName, containerName);
        }

        public async Task<Recipe> AddAsync(Recipe recipe)
        {
            var response = await container.CreateItemAsync<Recipe>(recipe, new PartitionKey(recipe.Id));
            return response.Resource;
        }

        public async Task UpdateAsync(Recipe recipe)
        {
            await container.UpsertItemAsync<Recipe>(recipe, new PartitionKey(recipe.Id));
        }        
        
        public async Task<Recipe> GetAsync(string recipeId)
        {
            try
            {
                var response = await container.ReadItemAsync<Recipe>(recipeId, new PartitionKey(recipeId));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new RecipeNotFoundInRepositoryException(recipeId);
            }
        }

        public async Task<IEnumerable<RecipeSummaryDto>> GetItemsAsync(int page, int pageSize)
        {
            var query = container.GetItemQueryIterator<RecipeSummaryDto>(
                new QueryDefinition("SELECT r.id, r.Name, r.ShortDescription FROM Recipes r OFFSET @skip LIMIT @take")
                    .WithParameter("@skip", (page -1) * pageSize) 
                    .WithParameter("@take", pageSize), 
                null, 
                new QueryRequestOptions { ConsistencyLevel = ConsistencyLevel.Session, MaxItemCount = pageSize });
            
            var results = new List<RecipeSummaryDto>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }

            return results;
        }
    }
}
