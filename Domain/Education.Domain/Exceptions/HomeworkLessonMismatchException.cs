using Education.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Domain.Exceptions
{
    /// <summary>
    /// Исключение, выбрасываемое при попытке привязать задание к "чужому" уроку.
    /// </summary>
    public class HomeworkLessonMismatchException(Lesson expectedLesson, Homework homework)
        : InvalidOperationException($"Cannot assign homework {homework.Id} to lesson {expectedLesson.Id} — it is linked to another lesson ({homework.Lesson.Id}).")
    {
        public Lesson ExpectedLesson => expectedLesson;
        public Homework Homework => homework;
    }
}
