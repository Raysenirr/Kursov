using Education.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Domain.Exceptions;
public class LessonCanselledException(Lesson lesson) : InvalidOperationException($"Lesson {lesson.Id} has been canselled yet")
{
    public Lesson Lesson => lesson;

}
