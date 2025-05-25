using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.ValueObjects.Exceptions
{
    public class LessonTopicDoubleWhitespaceException(string value)
        : ArgumentException("Lesson topic must not contain double whitespaces.", nameof(value))
    { }
}
