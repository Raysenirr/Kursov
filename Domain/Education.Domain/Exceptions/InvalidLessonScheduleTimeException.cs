using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Domain.Exceptions
{
    public class InvalidLessonScheduleTimeException(DateTime time) : ArgumentException("Lesson can not be reschedulled to past", nameof(time))
    {
        public DateTime Time => time;

    }
}
