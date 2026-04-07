using System;
using System.Collections.Generic;
using System.Text;

namespace JobTracker.Application.Common.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string entityName, object key)
        : base($"{entityName} con id '{key}' no fue encontrado.")
        {
        }
    }
}
