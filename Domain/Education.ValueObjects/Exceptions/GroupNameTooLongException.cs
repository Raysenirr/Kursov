using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.ValueObjects.Exceptions
{
    public class GroupNameTooLongException(string value, int maxLength)
        : ArgumentOutOfRangeException(nameof(value), value, $"Group name must be less than {maxLength} characters.")
    {
        public string Value => value;
        public int MaxLength => maxLength;
    }
}
