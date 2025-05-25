using Education.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Domain.Exceptions;

public class AnotherTeacherLessonGradedException(Lesson lesson, Teacher teacher) : InvalidOperationException($"Teacher {teacher.Name} has not teached lesson {lesson.Id}")
{
    public Lesson Lesson => lesson;
    public Teacher Teacher => teacher;

}
