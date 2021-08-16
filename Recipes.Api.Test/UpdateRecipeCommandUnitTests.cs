using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Recipes.Core.Application.Contracts;
using Recipes.Core.Application.Exceptions;
using Recipes.Core.Application.Features.Recipes.Commands.UpdateRecipe;
using Recipes.Core.Domain.Entities;

namespace Recipes.Api.Test
{
    [TestClass]
    public class UpdateRecipeCommandUnitTests
    {
        private const string VALID_RECIPE_ID = "rcp1";
        private Mock<IRecipeRepository> testRecipeRepository;
        private UpdateRecipeCommandHandler commandHandler;

        [TestInitialize]
        public void Init()
        {
            testRecipeRepository = new Mock<IRecipeRepository>();
            testRecipeRepository.Setup(r => r.GetAsync(It.Is<string>(s => s.Equals(VALID_RECIPE_ID)))).Returns(Task.FromResult(new Recipe()));
            testRecipeRepository.Setup(r => r.UpdateAsync(It.IsAny<Recipe>())).Returns(Task.CompletedTask);
            commandHandler = new UpdateRecipeCommandHandler(testRecipeRepository.Object);
        }

        [TestMethod]
        public async Task UpdateRecipeCommandHandler_Success()
        {
            // arrange
            var command = new UpdateRecipeCommand 
            { 
                Id = VALID_RECIPE_ID,
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
            testRecipeRepository.Verify(r => r.UpdateAsync(It.IsAny<Recipe>()), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public async Task UpdateRecipeCommandValidator_without_name_throws_ValidationException()
        {
            // arrange
            var commandWithoutName = new UpdateRecipeCommand
            {
                Id = "rcp1",
                // Name = missing
                Description = "Recipe description",
                ShortDescription = "Recipe short description",
                Portions = 3,
                PreparationTime = 25,
                ImageUrl = "https://www.dummy.com/img.png"
            };

            // act
            await commandHandler.Handle(commandWithoutName, new CancellationToken());

            // assert
            // FluenValidation.ValidationException
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task UpdateRecipeCommandValidator_without_valid_id_throws_NotFoundException()
        {
            // arrange
            var commandWithoutName = new UpdateRecipeCommand
            {
                Id = "rcp2",
                Name = "Recipe name",
                Description = "Recipe description",
                ShortDescription = "Recipe short description",
                Portions = 3,
                PreparationTime = 25,
                ImageUrl = "https://www.dummy.com/img.png"
            };

            // act
            await commandHandler.Handle(commandWithoutName, new CancellationToken());

            // assert
            // NotFoundException
        }
    }
}
