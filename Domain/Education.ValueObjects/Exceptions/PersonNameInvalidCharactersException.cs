using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.ValueObjects.Exceptions
{
    public class PersonNameInvalidCharactersException(string value)
        : ArgumentException("Person name must contain only letters or whitespaces.", nameof(value))
    { }
}
