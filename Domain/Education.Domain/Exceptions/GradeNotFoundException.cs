using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Education.Domain.Entities;

namespace Education.Domain.Exceptions
{
    public class GradeNotFoundException(Student student, Lesson lesson)
        : InvalidOperationException($"Student {student.Name} does not have a grade for lesson {lesson.Id}.")
    {
        public Student Student => student;
        public Lesson Lesson => lesson;
    }
}
