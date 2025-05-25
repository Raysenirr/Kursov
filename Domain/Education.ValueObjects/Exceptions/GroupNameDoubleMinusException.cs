using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.ValueObjects.Exceptions
{
    public class GroupNameDoubleMinusException(string value)
        : ArgumentException("Group name must not contain double minus '--'.", nameof(value))
    {
        public string Value => value;
    }
}
