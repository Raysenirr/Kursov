using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Domain.Exceptions
{
    /// <summary>
    /// Исключение, выбрасываемое при недопустимых параметрах сдачи задания.
    /// </summary>
    public class InvalidHomeworkSubmissionException(string paramName)
        : ArgumentException(paramName)
    {
        public string ParamName => paramName;
    }
}