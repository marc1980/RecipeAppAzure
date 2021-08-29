using System;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Recipes.Core.Application;
using Recipes.Core.Application.Contracts;
using Recipes.Core.Application.Features.Recipes.Commands.CreateRecipe;
using Recipes.Core.Application.Features.Recipes.Commands.UpdateRecipe;
using Recipes.Core.Application.Features.Recipes.Queries.GetRecipeDetails;
using Recipes.Core.Application.Features.Recipes.Queries.GetRecipesList;
using Recipes.Infrastructure;

namespace Recipes.Api
{
    public class Program
    {
        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults(
                //w =>
                //    w.UseMiddleware<ExceptionHandler>()
                )
                .ConfigureServices(s =>
                {
                    // s.AddLogging();

                    s.AddTransient<IFunctionExecutor, FunctionExecutor>();

                    s.AddCoreServices();

                    s.AddMediatR(typeof(CreateRecipeCommandHandler));
                    s.AddTransient<IValidator<CreateRecipeCommand>, CreateRecipeCommandValidator>();

                    s.AddMediatR(typeof(UpdateRecipeCommandHandler));
                    s.AddTransient<IValidator<UpdateRecipeCommand>, UpdateRecipeCommandValidator>();

                    s.AddMediatR(typeof(GetRecipeDetailsQueryHandler));
                    
                    s.AddMediatR(typeof(GetRecipeListQueryHandler));

                    // s.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

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