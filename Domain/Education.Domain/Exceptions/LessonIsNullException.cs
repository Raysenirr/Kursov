using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Domain.Exceptions
{
    public class LessonIsNullException()
        : ArgumentNullException("lesson", "Lesson cannot be null.")
    {
        public string Field => "lesson";
    }
}
