using MediatR;

namespace Recipes.Core.Application.Features.Recipes.Commands.AddReview
{
    public class AddReviewCommand : IRequest
    {
        public string RecipeId { get; set; }
        public string Reviewer { get; set; }
        public string Body { get; set; }
    }
}
