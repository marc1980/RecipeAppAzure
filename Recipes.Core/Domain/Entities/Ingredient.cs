using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace Recipes.Core.Domain.Entities
{
    //[JsonConverter(typeof(StringEnumConverter))]
    //public enum UnitOfMeasure
    //{
    //    Piece,
    //    Gram,
    //    Miligram,
    //    Kilogram,
    //    Liter,
    //    Mililiter,
    //    Centiliter
    //}

    public class Ingredient
    {
        public string Name { get; set; }
        public int Amount { get; set; }
       // public UnitOfMeasure Unit { get; set; }
    }
}
