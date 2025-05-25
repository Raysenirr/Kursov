using Education.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Domain.Exceptions;
public class DoubleVisitedLessonException(Lesson lesson, Student student) : InvalidOperationException($"Lesson {lesson.Id} has been visited by student ${student.Name} yet")
{
    public Lesson Lesson => lesson;
    public Student Student => student;

}
