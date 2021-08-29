using System;

namespace Recipes.Core.Application.Exceptions
{
    public class RecipeNotFoundInRepositoryException : ApplicationException 
    {
        public RecipeNotFoundInRepositoryException(string id) : base($"Recipe with Id {id} not found in repository") { }
    }
}
