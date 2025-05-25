using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.ValueObjects.Exceptions
{
    /// <summary>
    /// Исключение при слишком коротком заголовке домашнего задания.
    /// </summary>
    public class HomeworkTitleTooShortException(string value, int minLength)
        : ArgumentOutOfRangeException(nameof(value), value, $"Homework title must be at least {minLength} characters long.")
    {
        public string Value => value;
        public int MinLength => minLength;
    }
}
