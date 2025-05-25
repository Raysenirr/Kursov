using Education.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Domain.Exceptions;
public class DoubleGradeStudentLesson(Lesson lesson, Student student) : InvalidOperationException($"Student {student.Name} has been graded for lsson {lesson.Id} yet")
{
    public Lesson Lesson => lesson;
    public Student Student => student;

}
