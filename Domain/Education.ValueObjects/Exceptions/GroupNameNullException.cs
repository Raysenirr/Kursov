using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.ValueObjects.Exceptions
{
    /// <summary>
    /// Исключение, выбрасываемое при попытке использовать null в названии группы.
    /// </summary>
    public class GroupNameNullException(string paramName)
        : ArgumentNullException(paramName, "Group name must not be null.")
    {
        public string ParameterName => paramName;
    }
}
