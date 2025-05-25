using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.ValueObjects.Exceptions
{
    public class LessonTopicTooLongException(string value, int maxLength)
        : ArgumentOutOfRangeException(nameof(value), value, $"Lesson topic must be less than {maxLength} characters.")
    { }
}
