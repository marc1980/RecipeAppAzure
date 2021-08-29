using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Recipes.Core.Application.Contracts;
using Recipes.Core.Application.Exceptions;
using Recipes.Core.Domain.Entities;

namespace Recipes.Core.Application.Features.Recipes.Commands.AddReview
{
    public class AddReviewCommandHandler : IRequestHandler<AddReviewCommand, Unit>
    {
        private readonly IRecipeRepository recipeRepository;
        private readonly IMapper mapper;
        private readonly ILogger<AddReviewCommandHandler> logger;

        public AddReviewCommandHandler(IRecipeRepository recipeRepository, IMapper mapper, ILogger<AddReviewCommandHandler> logger)
        {
            this.recipeRepository = recipeRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<Unit> Handle(AddReviewCommand request, CancellationToken cancellationToken)
        {
            var validator = new AddReviewCommandValidator(recipeRepository);
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationApiException(validationResult);
            }

            var recipe = await recipeRepository.GetAsync(request.RecipeId);
            var review = mapper.Map<Review>(request);
            recipe.AddReview(review);

            try
            {
                await recipeRepository.UpdateAsync(recipe);
                return await Task.FromResult(Unit.Value);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
