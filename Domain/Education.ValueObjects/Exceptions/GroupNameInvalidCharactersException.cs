using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.ValueObjects.Exceptions
{
    /// <summary>
    /// Исключение, выбрасываемое при недопустимых символах в названии группы.
    /// </summary>
    public class GroupNameInvalidCharactersException(string value)
        : ArgumentException("Group name must contain only letters, digits or single minus characters.", nameof(value))
    {
        public string Value => value;
    }
}
