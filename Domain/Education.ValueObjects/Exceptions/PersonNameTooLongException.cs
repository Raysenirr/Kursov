using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.ValueObjects.Exceptions
{
    public class PersonNameTooLongException(string value, int maxLength)
        : ArgumentOutOfRangeException(nameof(value), value, $"Person name must be less than {maxLength} characters.")
    { }
}
