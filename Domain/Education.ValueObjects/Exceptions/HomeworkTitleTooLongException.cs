using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.ValueObjects.Exceptions
{
    /// <summary>
    /// Исключение при превышении допустимой длины заголовка домашнего задания.
    /// </summary>
    public class HomeworkTitleTooLongException(string value, int maxLength)
        : ArgumentOutOfRangeException(nameof(value), value, $"Homework title must not exceed {maxLength} characters.")
    {
        public string Value => value;
        public int MaxLength => maxLength;
    }
}
