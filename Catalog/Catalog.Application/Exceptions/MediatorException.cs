using System;

namespace Catalog.Application.Exceptions
{
    public sealed class MediatorException : Exception
    {
        public MediatorException(string message) : base(message)
        {
        }
    }
}