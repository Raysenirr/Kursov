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
        #region Свойства
        /// <summary> Домашнее задание, которое сдал студент </summary>
        public Homework Homework { get; }

        /// <summary> Студент, который сдал задание </summary>
        public Student Student { get; }

        /// <summary> Дата и время, когда задание было сдано </summary>
        public DateTime SubmittedAt { get; }

        /// <summary> Флаг, указывающий, было ли задание сдано с опозданием </summary>
        public bool IsLate { get; }
        #endregion

        #region Конструкторы

        public SubmittedHomeworkInfo(Homework homework, Student student, DateTime submittedAt)
        {
            Homework = homework ?? throw new HomeworkIsNullException();
            Student = student ?? throw new StudentIsNullException();
            SubmittedAt = submittedAt;
            IsLate = submittedAt > homework.Lesson.ClassTime.AddSeconds(1);
        }

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
        #endregion
    }
}