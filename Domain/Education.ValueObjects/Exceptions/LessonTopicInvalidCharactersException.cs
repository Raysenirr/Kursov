using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.ValueObjects.Exceptions
{
    public class LessonTopicInvalidCharactersException(string value)
        : ArgumentException("Lesson topic must contain only letters or whitespaces.", nameof(value))
    { }
}
