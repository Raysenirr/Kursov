using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.ValueObjects.Exceptions
{
    public class LessonTopicNullOrWhiteSpaceException(string paramName)
       : ArgumentNullException(paramName, "Lesson topic must not be null or whitespace.")
    { }
}
