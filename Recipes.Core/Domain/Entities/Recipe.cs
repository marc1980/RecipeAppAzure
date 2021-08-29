using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

namespace Recipes.Core.Domain.Entities
{
    public class Recipe
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public int PreparationTime { get; set; }
        public int Portions { get; set; }
        public IEnumerable<Ingredient> Ingredients { get; set; }
        public IEnumerable<PreparationStep> PreparationSteps { get; set; }
        public IEnumerable<Review> Reviews { get; private set; }

        public Recipe()
        {
            Ingredients = new List<Ingredient>();
            PreparationSteps = new List<PreparationStep>();
            Reviews = new List<Review>();
        }

        public void AddReview(Review review)
        {
            Reviews = Reviews.Append(review);
            return;
        }
    }
}
