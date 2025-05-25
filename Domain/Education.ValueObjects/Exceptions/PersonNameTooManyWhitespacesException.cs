using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.ValueObjects.Exceptions
{
    public class PersonNameTooManyWhitespacesException(string value, int maxWhitespaces)
        : ArgumentException($"Person name must contain less than {maxWhitespaces} whitespaces.", nameof(value))
    { }
}
