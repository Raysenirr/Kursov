using Education.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Domain.Exceptions
{
    public class AssignHomeworkToAnotherTeacherException(Lesson lesson, Teacher teacher)
        : InvalidOperationException($"Teacher {teacher.Name} cannot assign homework to lesson {lesson.Id} — it doesn't belong to them.")
    {
        public Lesson Lesson => lesson;
        public Teacher Teacher => teacher;
    }
}
