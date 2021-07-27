using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Recipes.Infrastructure.Repositories;

namespace Recipes.Infrastructure
{
    public static class CosmosDbClient
    {
        public static async Task<RecipeRepository> InitializeCosmosClientInstanceAsync(string connectionString, string databaseName, string containerName)
        {
            var client = new CosmosClient(connectionString);
            var recipeRepository = new RecipeRepository(client, databaseName, containerName);
            var database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
            _ = await database.Database.CreateContainerIfNotExistsAsync(containerName, "/id");

            return recipeRepository;
        }
    }
}
