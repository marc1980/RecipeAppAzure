using Newtonsoft.Json;

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
        //public virtual IEnumerable<Ingredient> Ingredients { get; set; }
        //public virtual IEnumerable<PreparationStep> Steps { get; set; }
        //public virtual IEnumerable<Review> Reviews { get; set; }

        public Recipe()
        {
            //Ingredients = new List<Ingredient>();
            //Steps = new List<PreparationStep>();
            //Reviews = new List<Review>();
        }
    }
}
