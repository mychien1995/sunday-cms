using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.Core.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException() : base()
        {

        }
        public EntityNotFoundException(string message) : base(message)
        {

        }
    }
}
