using System;
using System.Collections.Generic;
using System.Text;

namespace Loans.Application.Exceptions
{
    public sealed class ForbiddenException : Exception
    {
        public ForbiddenException(string message) : base(message)
        {
        }
    }
}
