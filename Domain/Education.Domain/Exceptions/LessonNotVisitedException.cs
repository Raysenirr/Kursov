using Education.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Domain.Exceptions;
public class LessonNotVisitedException(Lesson lesson, Student student) : InvalidOperationException($"Student {student.Name} has not visited lesson {lesson.Id}")
{
    public Lesson Lesson => lesson;
    public Student Student => student;

}
