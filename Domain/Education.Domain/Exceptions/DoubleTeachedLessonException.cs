using Education.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Domain.Exceptions
{
    public class DoubleTeachedLessonException(Lesson lesson, Teacher teacher) : InvalidOperationException($"Teacher {teacher.Name} has been teached lesson {lesson.Id} yet")
    {
        public Lesson Lesson => lesson;
        public Teacher Teacher => teacher;

    }
}
