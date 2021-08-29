using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Recipes.Core.Application;
using Recipes.Core.Application.Contracts;
using Recipes.Core.Application.Exceptions;
using Recipes.Core.Application.Features.Recipes.Queries.GetRecipeDetails;
using Recipes.Core.Domain.Entities;

namespace Recipes.Core.Test
{
    [TestClass]
    public class GetRecipeDetailsQueryUnitTests
    {
        private const string VALID_RECIPE_ID = "rcp1";
        private Mock<IRecipeRepository> testRecipeRepository;
        private IMapper Mapper;
        private GetRecipeDetailsQueryHandler queryHandler;

        [TestInitialize]
        public void Init()
        {
            testRecipeRepository = new Mock<IRecipeRepository>();
            testRecipeRepository.Setup(r => r.GetAsync(It.Is<string>(s => !s.Equals(VALID_RECIPE_ID)))).Throws(new RecipeNotFoundInRepositoryException("id"));

            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfiles>());
            Mapper = mapperConfig.CreateMapper();

            var logger = new Mock<ILogger<GetRecipeDetailsQueryHandler>>();

            queryHandler = new GetRecipeDetailsQueryHandler(testRecipeRepository.Object, logger.Object);
        }

        [TestMethod]
        public async Task GetRecipeQueryHandler_Success()
        {
            // arrange
            var expectedName = "Name ";
            var expectedDescription = "Description";
            var expectedShortDescription = "Short Description";
            var expectedImageUrl = "http://host.com/image.png";
            var expectedPortions = 3;
            var expectedPreparationTime = 20;
            var expectedIngredients = new List<Ingredient>() { new Ingredient { Name = "Ingredient1", Amount = 2 } };
            var expectedPreparationSteps = new List<PreparationStep>() { new PreparationStep { Rank = 1, Description = "Preparationstep1" } };
            
            var expectedRecipe = new Recipe
            {
                Id = VALID_RECIPE_ID,
                Name = expectedName,
                Description = expectedDescription,
                ShortDescription = expectedShortDescription,
                ImageUrl = expectedImageUrl,
                Portions = expectedPortions,
                PreparationTime = expectedPreparationTime,
                Ingredients = expectedIngredients,
                PreparationSteps = expectedPreparationSteps
            };

            testRecipeRepository.Setup(r => r.GetAsync(It.Is<string>(s => s.Equals(VALID_RECIPE_ID)))).Returns(Task.FromResult(expectedRecipe));

            var command = new GetRecipeDetailsQuery
            {
                Id = VALID_RECIPE_ID,
            };

            // act
            var result = await queryHandler.Handle(command, new CancellationToken());

            // assert
            testRecipeRepository.Verify(r => r.GetAsync(It.IsAny<string>()), Times.Once());
            Assert.AreEqual(VALID_RECIPE_ID, result.Id);
            Assert.AreEqual(expectedName, result.Name);
            Assert.AreEqual(expectedDescription, result.Description);
            Assert.AreEqual(expectedShortDescription, result.ShortDescription);
            Assert.AreEqual(expectedImageUrl, result.ImageUrl);
            Assert.AreEqual(expectedPortions, result.Portions);
            Assert.AreEqual(expectedPreparationTime, result.PreparationTime);
            Assert.AreEqual(expectedIngredients.First().Name, result.Ingredients.First().Name);
            Assert.AreEqual(expectedIngredients.First().Amount, result.Ingredients.First().Amount);
            Assert.AreEqual(expectedPreparationSteps.First().Rank, result.PreparationSteps.First().Rank);
            Assert.AreEqual(expectedPreparationSteps.First().Description, result.PreparationSteps.First().Description);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundApiException))]
        public async Task GetRecipeQueryValidator_without_valid_id_throws_NotFoundException()
        {
            // arrange
            var queryWithoutValidId = new GetRecipeDetailsQuery
            {
                Id = "rcp2",
            };

            // act
            await queryHandler.Handle(queryWithoutValidId, new CancellationToken());

            // assert
            // NotFoundException
        }
    }
}
