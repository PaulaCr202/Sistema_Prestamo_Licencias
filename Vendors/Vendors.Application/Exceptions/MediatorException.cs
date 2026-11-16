using System;
using System.Collections.Generic;
using System.Text;


namespace Vendors.Application.Exceptions
{
    public class MediatorException : Exception
    {
        public MediatorException(string message)
            : base(message)
        {
        }
    }
}
