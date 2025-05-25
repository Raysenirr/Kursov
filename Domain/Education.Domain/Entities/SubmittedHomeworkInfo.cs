using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Education.Domain.Entities.Base;
using Education.Domain.Exceptions;

namespace Education.Domain.Entities
{
    /// <summary>
    /// Информация о сданном домашнем задании
    /// </summary>
    public class SubmittedHomeworkInfo
    {
        public Homework Homework { get; }
        public Student Student { get; }
        public DateTime SubmittedAt { get; }
        public bool IsLate { get; }

        /// <summary>
        /// Основной конструктор
        /// </summary>
        public SubmittedHomeworkInfo(Homework homework, Student student, DateTime submittedAt)
        {
            Homework = homework ?? throw new HomeworkIsNullException();
            Student = student ?? throw new StudentIsNullException();
            SubmittedAt = submittedAt;
            IsLate = submittedAt > homework.Lesson.ClassTime.AddSeconds(1);
        }

        /// <summary>
        /// Для БД
        /// </summary>
        public SubmittedHomeworkInfo(HomeworkSubmission submission)
            : this(submission.Homework, submission.Student, submission.SubmissionDate)
        {
        }

        /// <summary>
        /// Конструктор для EF 
        /// </summary>
        private SubmittedHomeworkInfo()
        {
            Homework = null!;
            Student = null!;
        }
    }
}