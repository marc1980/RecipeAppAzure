using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace Recipes.Core.Application.Exceptions
{
    public class ValidationException : ApplicationException
    {
        public List<string> ValidationErrors { get; private set; }

        public ValidationException(ValidationResult validationResult)
        {
            ValidationErrors = validationResult.Errors
                .Select(e => e.ErrorMessage)
                .ToList();
        }
    }
}
