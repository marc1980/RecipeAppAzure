using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Recipes.Core.Application;
using Recipes.Core.Application.Contracts;
using Recipes.Core.Application.Exceptions;
using Recipes.Core.Application.Features.Recipes.Commands.AddReview;
using Recipes.Core.Domain.Entities;

namespace Recipes.Core.Test
{
    [TestClass]
    public class AddReviewCommandUnitTests
    {
        private const string VALID_RECIPE_ID = "rcp1";
        private Mock<IRecipeRepository> testRecipeRepository;
        private IMapper Mapper;
        private AddReviewCommandHandler commandHandler;

        [TestInitialize]
        public void Init()
        {
            testRecipeRepository = new Mock<IRecipeRepository>();
            testRecipeRepository.Setup(r => r.GetAsync(It.Is<string>(s => s.Equals(VALID_RECIPE_ID)))).Returns(Task.FromResult(new Recipe() { Id = VALID_RECIPE_ID }));
            testRecipeRepository.Setup(r => r.GetAsync(It.Is<string>(s => !s.Equals(VALID_RECIPE_ID)))).Throws(new RecipeNotFoundInRepositoryException("id"));
            testRecipeRepository.Setup(r => r.UpdateAsync(It.IsAny<Recipe>())).Returns(Task.CompletedTask);

            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfiles>());
            Mapper = mapperConfig.CreateMapper();

            var logger = new Mock<ILogger<AddReviewCommandHandler>>();

            commandHandler = new AddReviewCommandHandler(testRecipeRepository.Object, Mapper, logger.Object);
        }

        [TestMethod]
        public async Task UpdateRecipeCommandHandler_Success()
        {
            // arrange
            var command = new AddReviewCommand
            {
                RecipeId = VALID_RECIPE_ID,
                Reviewer = "Marc",
                Body = "My review"
            };

            // act
            var result = await commandHandler.Handle(command, new CancellationToken());

            // assert
            testRecipeRepository.Verify(r => r.UpdateAsync(It.IsAny<Recipe>()), Times.Once());
        }


        [TestMethod]
        [ExpectedException(typeof(ValidationApiException))]
        public async Task AddReviewCommandValidator_without_reviewer_throws_ValidationException()
        {
            // arrange
            var commandWithoutReviewer = new AddReviewCommand
            {
                RecipeId = VALID_RECIPE_ID,
                Body = "My review"
            };

            // act
            await commandHandler.Handle(commandWithoutReviewer, new CancellationToken());

            // assert
            // FluenValidation.ValidationException
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundApiException))]
        public async Task AddReviewCommandValidator_without_valid_id_throws_NotFoundException()
        {
            // arrange
            var commandWithoutValidId = new AddReviewCommand
            {
                RecipeId = "rcp2",
                Reviewer = "Marc",
                Body = "My review"
            };

            // act
            await commandHandler.Handle(commandWithoutValidId, new CancellationToken());

            // assert
            // NotFoundException
        }
    }
}
