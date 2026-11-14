using System;
using System.Collections.Generic;
using System.Text;

namespace Loans.Application.Exceptions
{
    public sealed class MediatorException : Exception
    {
        public MediatorException(string message) : base(message)
        {
        }
    }
}
