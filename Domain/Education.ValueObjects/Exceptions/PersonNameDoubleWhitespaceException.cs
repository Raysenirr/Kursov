using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.ValueObjects.Exceptions
{
    public class PersonNameDoubleWhitespaceException(string value)
        : ArgumentException("Person name must not contain double whitespaces.", nameof(value))
    { }

}
