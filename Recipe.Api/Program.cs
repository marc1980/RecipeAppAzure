using System;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Recipes.Core.Application.Contracts;
using Recipes.Core.Application.Features.Recipes.Commands.CreateRecipe;
using Recipes.Infrastructure;

namespace Recipes.Api
{
    public class Program
    {
        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices(s =>
                {
                    // s.AddLogging();
                    s.AddMediatR(typeof(CreateRecipeCommandHandler));
                   // s.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
                    s.AddSingleton<IValidator<CreateRecipeCommand>, CreateRecipeCommandValidator>();
                    s.AddSingleton<IRecipeRepository>(CosmosDbClient.InitializeCosmosClientInstanceAsync(
                        GetEnvironmentVariable("ConnectionStrings:CosmosDbConnectionString"), 
                        GetEnvironmentVariable("CosmosDbDatabaseName"), 
                        GetEnvironmentVariable("CosmosDbContainerName"))
                        .GetAwaiter()
                        .GetResult());
                })
                .Build();

            host.Run();
        }

        private static string GetEnvironmentVariable(string key)
        {
            return Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.Process);
        }
    }
}