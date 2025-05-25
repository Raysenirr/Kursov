using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Education.Domain.Exceptions;
using Education.Domain.Entities;

namespace Education.Domain.Exceptions
{
    public class LateHomeworkSubmissionException(Student student, Homework homework)
        : InvalidOperationException($"Student {student.Name.Value} submitted homework {homework.Title.Value} after the lesson date.")
    {
        public Student Student { get; } = student;
        public Homework Homework { get; } = homework;
    }
}