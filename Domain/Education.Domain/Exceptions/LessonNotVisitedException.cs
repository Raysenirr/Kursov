using Education.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Domain.Exceptions;
public class LessonNotVisitedException : InvalidOperationException
{
    public Lesson Lesson { get; }
    public Student Student { get; }

    public LessonNotVisitedException(Lesson lesson, Student student)
        : base($"Student {student?.Name?.Value ?? "UNKNOWN"} has not visited lesson {lesson?.Id.ToString() ?? "UNKNOWN"}")
    {
        Lesson = lesson;
        Student = student;
    }
}

