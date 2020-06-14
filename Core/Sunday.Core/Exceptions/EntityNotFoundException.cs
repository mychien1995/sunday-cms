using System;

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
