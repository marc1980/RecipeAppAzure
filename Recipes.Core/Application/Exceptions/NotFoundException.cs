using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace Recipes.Core.Application.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string name, string id) : base($"{name} / {id} not found.") { }
    }
}