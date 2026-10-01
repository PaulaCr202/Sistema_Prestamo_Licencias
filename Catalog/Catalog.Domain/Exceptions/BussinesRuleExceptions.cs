using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Domain.Exceptions
{
    internal class BussinesRuleException : Exception
    {
        public BussinesRuleException(string message)
            : base(message)
        {
        }
    }
}
