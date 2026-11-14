using System;
using System.Collections.Generic;
using System.Text;

namespace Loans.Application.Exceptions
{
    public sealed class ExternalServiceException : Exception
    {
        public ExternalServiceException(string message) : base(message)
        {
        }
    }
}
