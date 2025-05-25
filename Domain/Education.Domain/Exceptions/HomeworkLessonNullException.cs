using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Domain.Exceptions
{
    /// <summary>
    /// Исключение, выбрасываемое при создании домашнего задания без привязки к уроку.
    /// </summary>
    public class HomeworkLessonNullException()
        : ArgumentNullException("lesson", "Homework must be linked to a lesson.")
    {
        public string ParameterName => "lesson";
    }
}