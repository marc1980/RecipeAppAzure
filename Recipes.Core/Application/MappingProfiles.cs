using AutoMapper;
using Recipes.Core.Application.Features.Recipes.Commands.AddReview;
using Recipes.Core.Application.Features.Recipes.Commands.CreateRecipe;
using Recipes.Core.Application.Features.Recipes.Commands.UpdateRecipe;
using Recipes.Core.Domain.Entities;

namespace Recipes.Core.Application
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Recipe, CreateRecipeCommand>().ReverseMap();
            CreateMap<Recipe, UpdateRecipeCommand>().ReverseMap();
            CreateMap<Review, AddReviewCommand>().ReverseMap();
        }
    }
}
