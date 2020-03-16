using System;

namespace WingsOn.Data.Logic.Exceptions
{
    public class PersonNotExistsException : Exception
    {
        public PersonNotExistsException(string message) : base(message)
        {
        }

        public PersonNotExistsException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
