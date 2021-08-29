using System;

namespace Recipes.Core.Application.Exceptions
{
    public class NotFoundApiException : ApplicationException
    {
        public NotFoundApiException(string name, string id) : base($"{name} / {id} not found.") { }
    }
}