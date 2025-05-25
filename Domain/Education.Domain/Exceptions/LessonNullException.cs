using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Domain.Exceptions
{
    public class LessonNullException()
        : ArgumentNullException("lesson", "Lesson must not be null.")
    { }
}
