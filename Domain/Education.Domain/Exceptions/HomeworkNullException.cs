using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Domain.Exceptions
{
    /// <summary>
    /// Исключение, выбрасываемое при передаче null вместо домашнего задания.
    /// </summary>
    public class HomeworkNullException()
        : InvalidOperationException("Homework must not be null when checking or processing.")
    {
    }
}
