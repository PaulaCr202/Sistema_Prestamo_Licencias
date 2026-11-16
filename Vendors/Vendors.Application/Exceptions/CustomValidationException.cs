using System;
using System.Collections.Generic;
using System.Text;


using FluentValidation.Results;

namespace Vendors.Application.Exceptions
{
    public class CustomValidationException : Exception
    {
        public IReadOnlyCollection<ValidationFailure> Errors { get; set; } = [];

        public CustomValidationException(
            IReadOnlyCollection<ValidationFailure> failures)
        {
            Errors = failures;
        }
    }
}

