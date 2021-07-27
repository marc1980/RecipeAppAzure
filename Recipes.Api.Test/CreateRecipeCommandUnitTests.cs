using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Recipes.Core.Application.Contracts;
using Recipes.Core.Application.Features.Recipes.Commands.CreateRecipe;
using Recipes.Core.Domain.Entities;

namespace Recipes.Api.Test
{
    [TestClass]
    public class CreateRecipeCommandUnitTests
    {
        private Mock<IRecipeRepository> testRecipeRepository;
        private CreateRecipeCommandHandler commandHandler;

        [TestInitialize]
        public void Init()
        {
            testRecipeRepository = new Mock<IRecipeRepository>();
            testRecipeRepository.Setup(r => r.AddAsync(It.IsAny<Recipe>())).Returns(Task.CompletedTask);
            commandHandler = new CreateRecipeCommandHandler(testRecipeRepository.Object);
        }

        [TestMethod]
        public async Task CreateRecipeCommandHandler_Success()
        {
            // arrange
            var command = new CreateRecipeCommand 
            { 
                Name = "Recipe name", 
                Description = "Recipe description", 
                ShortDescription = "Recipe short description", 
                Portions = 3, 
                PreparationTime = 25, 
                ImageUrl = "https://www.dummy.com/img.png" 
            };

            // act
            await commandHandler.Handle(command, new CancellationToken());

            // assert
            testRecipeRepository.Verify(r => r.AddAsync(It.IsAny<Recipe>()), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public async Task CreateRecipeCommandValidator_Throws_ValidationException()
        {
            // arrange
            var command = new CreateRecipeCommand
            {
                Description = "Recipe description",
                ShortDescription = "Recipe short description",
                Portions = 3,
                PreparationTime = 25,
                ImageUrl = "https://www.dummy.com/img.png"
            };

            // act
            await commandHandler.Handle(command, new CancellationToken());

            // assert
            // FluenValidation.ValidationException
        }

    }
}
