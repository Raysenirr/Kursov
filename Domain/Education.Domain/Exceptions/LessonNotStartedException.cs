using Education.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Domain.Exceptions;
public class LessonNotStartedException(Lesson lesson) : InvalidOperationException($"Lesson {lesson.Id} not started yet")
{
    public Lesson Lesson => lesson;

}
