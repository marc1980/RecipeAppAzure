using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Recipes.Core.Application;
using Recipes.Core.Application.Contracts;
using Recipes.Core.Application.Exceptions;
using Recipes.Core.Application.Features.Recipes.Commands.UpdateRecipe;
using Recipes.Core.Domain.Entities;

namespace Recipes.Core.Test
{
    [TestClass]
    public class UpdateRecipeCommandUnitTests
    {
        private const string VALID_RECIPE_ID = "rcp1";
        private Mock<IRecipeRepository> testRecipeRepository;
        private IMapper Mapper;
        private UpdateRecipeCommandHandler commandHandler;

        [TestInitialize]
        public void Init()
        {
            testRecipeRepository = new Mock<IRecipeRepository>();
            testRecipeRepository.Setup(r => r.GetAsync(It.Is<string>(s => s.Equals(VALID_RECIPE_ID)))).Returns(Task.FromResult(new Recipe()));
            testRecipeRepository.Setup(r => r.GetAsync(It.Is<string>(s => !s.Equals(VALID_RECIPE_ID)))).Throws(new RecipeNotFoundInRepositoryException("id"));
            testRecipeRepository.Setup(r => r.UpdateAsync(It.IsAny<Recipe>())).Returns(Task.CompletedTask);

            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfiles>());
            Mapper = mapperConfig.CreateMapper();

            var logger = new Mock<ILogger<UpdateRecipeCommandHandler>>();

            commandHandler = new UpdateRecipeCommandHandler(testRecipeRepository.Object, Mapper, logger.Object);
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
                ImageUrl = "https://www.dummy.com/img.png",
                Ingredients = new List<Ingredient>() { new Ingredient { Name = "Ingredient name", Amount = 2 } },
                PreparationSteps = new List<PreparationStep>() { new PreparationStep { Rank = 1, Description = "Step 1" } }
            };

            // act
            var result = await commandHandler.Handle(command, new CancellationToken());

            // assert
            testRecipeRepository.Verify(r => r.UpdateAsync(It.IsAny<Recipe>()), Times.Once());
            Assert.IsTrue(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationApiException))]
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
        [ExpectedException(typeof(NotFoundApiException))]
        public async Task UpdateRecipeCommandValidator_without_valid_id_throws_NotFoundException()
        {
            // arrange
            var commandWithoutValidId = new UpdateRecipeCommand
            {
                Id = "rcp2",
                Name = "Recipe name",
                Description = "Recipe description",
                ShortDescription = "Recipe short description",
                Portions = 3,
                PreparationTime = 25,
                ImageUrl = "https://www.dummy.com/img.png",
                Ingredients = new List<Ingredient>() { new Ingredient { Name = "Ingredient name", Amount = 2 } },
                PreparationSteps = new List<PreparationStep>() { new PreparationStep { Rank = 1, Description = "Step 1" } }
            };

            // act
            await commandHandler.Handle(commandWithoutValidId, new CancellationToken());

            // assert
            // NotFoundException
        }
    }
}
