using Education.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Domain.Exceptions
{
    /// <summary>
    /// Исключение, выбрасываемое при попытке задать домашнее задание до проведения урока.
    /// </summary>
   public class HomeworkBeforeLessonException(Lesson lesson)
        : InvalidOperationException($"Cannot assign homework to lesson {lesson.Id} — it has not been taught yet.")
    {
        public Lesson Lesson => lesson;
    }
}
