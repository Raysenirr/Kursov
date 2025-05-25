using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Domain.Exceptions
{
    /// <summary>
    /// Исключение при попытке задать дедлайн в прошлом.
    /// </summary>
    public class InvalidHomeworkDeadlineException(DateTime date)
        : ArgumentException($"Homework deadline can't be in the past: {date}.")
    {
        public DateTime Deadline => date;
    }
}
