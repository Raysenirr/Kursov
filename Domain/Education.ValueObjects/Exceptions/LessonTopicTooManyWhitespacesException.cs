using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.ValueObjects.Exceptions
{
    public class LessonTopicTooManyWhitespacesException(string value, int maxWhitespaces)
        : ArgumentException($"Lesson topic must contain less than {maxWhitespaces} whitespaces.", nameof(value))
    { }
}
