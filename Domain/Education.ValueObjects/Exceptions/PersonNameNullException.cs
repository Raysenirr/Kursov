using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.ValueObjects.Exceptions
{
    public class PersonNameNullException(string paramName)
            : ArgumentNullException(paramName, "Person name must not be null")
    { }
}
