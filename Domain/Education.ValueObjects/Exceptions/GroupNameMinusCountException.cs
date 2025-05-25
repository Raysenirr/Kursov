using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.ValueObjects.Exceptions
{
    /// <summary>
    /// Исключение, выбрасываемое при неправильном количестве дефисов в названии группы.
    /// </summary>
    public class GroupNameMinusCountException(string value, int requiredCount)
        : ArgumentException($"Group name must contain exactly {requiredCount} minus characters.", nameof(value))
    {
        public string Value => value;
        public int RequiredCount => requiredCount;
    }
}
