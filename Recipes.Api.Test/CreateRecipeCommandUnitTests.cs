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
using Recipes.Core.Application.Features.Recipes.Commands.CreateRecipe;
using Recipes.Core.Domain.Entities;

namespace Recipes.Core.Test
{
    [TestClass]
    public class CreateRecipeCommandUnitTests
    {
        private Mock<IRecipeRepository> testRecipeRepository;
        private CreateRecipeCommandHandler commandHandler;
        private IMapper Mapper;

        [TestInitialize]
        public void Init()
        {
            testRecipeRepository = new Mock<IRecipeRepository>();
            
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfiles>());
            Mapper = mapperConfig.CreateMapper();

            var logger = new Mock<ILogger<CreateRecipeCommandHandler>>();

            commandHandler = new CreateRecipeCommandHandler(testRecipeRepository.Object, Mapper, logger.Object);
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
                ImageUrl = "https://www.dummy.com/img.png",
                Ingredients = new List<Ingredient>() { new Ingredient { Name = "Ingredient name", Amount = 2 } },
                PreparationSteps = new List<PreparationStep>() { new PreparationStep { Rank = 1, Description = "Step 1" } }
            };

            testRecipeRepository.Setup(r => r.AddAsync(It.IsAny<Recipe>())).ReturnsAsync(
                new Recipe 
                { 
                    Id = "1", 
                    Name = command.Name, 
                    Description = command.Description,
                    ShortDescription = command.ShortDescription,
                    Portions = command.Portions,
                    PreparationTime = command.PreparationTime,
                    ImageUrl = command.ImageUrl,
                });


            // act
            var result = await commandHandler.Handle(command, new CancellationToken());

            // assert
            testRecipeRepository.Verify(r => r.AddAsync(It.IsAny<Recipe>()), Times.Once());
            Assert.AreEqual(command.Name, result.Name);
            Assert.AreEqual(command.ShortDescription, result.ShortDescription);
            Assert.AreEqual(command.Description, result.Description);
            Assert.AreEqual(command.ImageUrl, result.ImageUrl);
            Assert.AreEqual(command.Portions, result.Portions);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationApiException))]
        public async Task CreateRecipeCommandValidator_without_name_throws_ValidationException()
        {
            // arrange
            var commandWithoutName = new CreateRecipeCommand
            {
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
    }
}
