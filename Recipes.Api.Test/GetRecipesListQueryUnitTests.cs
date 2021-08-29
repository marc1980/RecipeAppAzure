using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Recipes.Core.Application.Contracts;
using Recipes.Core.Application.Features.Recipes.Queries.GetRecipesList;

namespace Recipes.Core.Test
{
    [TestClass]
    public class GetRecipesListQueryUnitTests
    {

        private Mock<IRecipeRepository> testRecipeRepository;
        private GetRecipeListQueryHandler queryHandler;
        private IEnumerable<RecipeSummaryDto> recipes = new List<RecipeSummaryDto>
        {
            new RecipeSummaryDto { Id = "rcp1", Name = "Name 1", ShortDescription = "Short Description 1" },
            new RecipeSummaryDto { Id = "rcp2", Name = "Name 2", ShortDescription = "Short Description 2" },
            new RecipeSummaryDto { Id = "rcp3", Name = "Name 3", ShortDescription = "Short Description 3" },
            new RecipeSummaryDto { Id = "rcp4", Name = "Name 4", ShortDescription = "Short Description 4" },
            new RecipeSummaryDto { Id = "rcp5", Name = "Name 5", ShortDescription = "Short Description 5" },
            new RecipeSummaryDto { Id = "rcp6", Name = "Name 6", ShortDescription = "Short Description 6" },
            new RecipeSummaryDto { Id = "rcp7", Name = "Name 7", ShortDescription = "Short Description 7" },
            new RecipeSummaryDto { Id = "rcp8", Name = "Name 8", ShortDescription = "Short Description 8" },
            new RecipeSummaryDto { Id = "rcp9", Name = "Name 9", ShortDescription = "Short Description 9" },
            new RecipeSummaryDto { Id = "rcp10", Name = "Name 10", ShortDescription = "Short Description 10" },
        };

        [TestInitialize]
        public void Init()
        {
            testRecipeRepository = new Mock<IRecipeRepository>();

            var logger = new Mock<ILogger<GetRecipeListQueryHandler>>();

            queryHandler = new GetRecipeListQueryHandler(testRecipeRepository.Object, logger.Object);
        }

        [TestMethod]
        [DataRow(1, 10, 10, new[] { "rcp1", "rcp2", "rcp3", "rcp4", "rcp5", "rcp6", "rcp7", "rcp8", "rcp9", "rcp10" })]
        [DataRow(2, 3, 3, new[] {  "rcp4", "rcp5", "rcp6" })]
        [DataRow(1, 100, 10, new[] { "rcp1", "rcp2", "rcp3", "rcp4", "rcp5", "rcp6", "rcp7", "rcp8", "rcp9", "rcp10" })]
        public async Task GetRecipeListQueryHandler_Success(int page, int pageSize, int expectedRecordcount, string[] expectedIds)
        {
            // arrange
            testRecipeRepository.Setup(r => r.GetItemsAsync(It.Is<int>(p => p == page), It.Is<int>(p => p == pageSize))).Returns(Task.FromResult(recipes.Skip((page -1) * pageSize).Take(pageSize)));

            var query = new GetRecipeListQuery
            {
                Page = page,
                PageSize = pageSize
            };

            // act
            var result = await queryHandler.Handle(query, new CancellationToken());

            // assert
            Assert.AreEqual(expectedRecordcount, result.ToList().Count);
            CollectionAssert.AreEqual(expectedIds, result.Select(r => r.Id).ToArray());
        }
    }
}
