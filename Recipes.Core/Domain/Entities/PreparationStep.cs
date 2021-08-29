namespace Recipes.Core.Domain.Entities
{
    public class PreparationStep
    {
        // [Range(1, 99)]
        public int Rank { get; set; }
        //[Required]
        //[StringLength(250)]
        public string Description { get; set; }
    }
}
